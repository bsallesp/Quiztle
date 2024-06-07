using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.API.Controllers.CourseControllers.QuestionControllers;
using BrunoTheBot.DataContext.Repositories;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.CoreBusiness.Entities.PDFData;
using BrunoTheBot.CoreBusiness.Utils;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePDFDataFromStreamController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CreateQuestionsFromBookController _createQuestionController;
        private readonly AILogRepository _aILogRepository;
        private readonly PDFDataRepository _pDFDataRepository;
        private readonly IConfiguration _configuration;

        public CreatePDFDataFromStreamController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            CreateQuestionsFromBookController createQuestionsController,
            AILogRepository aILogRepository,
            PDFDataRepository pDFDataRepository
            )
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _createQuestionController = createQuestionsController;
            _aILogRepository = aILogRepository;
            _pDFDataRepository = pDFDataRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] string fileName = "NEC2017.PDF", string name = "unnamed")
        {
            try
            {
                string filePath = _configuration["UploadPDFDirectoryByPythonAPI"]! ?? throw new Exception("CreatePDFDataFromStreamController: PDF DIRECTORY NOT FOUND");
                string completeFileTarget = filePath + fileName;

                var client = _httpClientFactory.CreateClient("PDFClient");

                var generalUUID = Guid.NewGuid();

                var newPDFData = new PDFData()
                {
                    Id = generalUUID,
                    FileName = name,
                    Created = DateTime.UtcNow,
                    Name = name
                };

                if (string.IsNullOrEmpty(filePath))
                    return BadRequest("File path not provided");

                Console.WriteLine($"completeFileTarget: {completeFileTarget}");
                var content = new MultipartFormDataContent
                {
                    { new StringContent(completeFileTarget), "file_path" },
                    { new StringContent("1"), "partial_output_rate" }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "extract-text-mupdf") { Content = content };
                Console.WriteLine($"Final request URL: {new Uri(client.BaseAddress!, "extract-text-mupdf")}");

                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                var count = 0;
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(line) && line.StartsWith("data:"))
                    {
                        var text = line.Substring(5);
                        Console.WriteLine("Received text part: " + text);
                        count++;
                        newPDFData.Pages.Add(new PDFDataPages
                        {
                            Content = text,
                            Page = count,
                            Created = DateTime.UtcNow
                        });
                    }
                }

                await _pDFDataRepository.CreatePDFDataAsync(newPDFData);

                return Ok("Processing complete");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}