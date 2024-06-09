using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.API.Controllers.CourseControllers.QuestionControllers;
using BrunoTheBot.DataContext.Repositories;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.CoreBusiness.Entities.PDFData;
using System.IO;

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

        public CreatePDFDataFromStreamController(
            IHttpClientFactory httpClientFactory,
            CreateQuestionsFromBookController createQuestionsController,
            AILogRepository aILogRepository,
            PDFDataRepository pDFDataRepository
            )
        {
            _httpClientFactory = httpClientFactory;
            _createQuestionController = createQuestionsController;
            _aILogRepository = aILogRepository;
            _pDFDataRepository = pDFDataRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromQuery] string fileName, [FromQuery] string pdfDataName)
        {
            try
            {
                Console.WriteLine("starting to extract...");
                string filePath = string.Empty;
                if (Directory.Exists("c:/bucket")) filePath = "c:/bucket/";
                if (Directory.Exists("/bucket")) filePath = "/bucket/";
                if (string.IsNullOrEmpty(filePath)) throw new Exception("BTB API - CreatePDFDataFromStreamController: filePath to get the file not found.");

                string completeFileTarget = filePath + fileName;
                Console.WriteLine($"Complete file path: {completeFileTarget}");

                var client = _httpClientFactory.CreateClient("PDFClient");

                var generalUUID = Guid.NewGuid();

                var newPDFData = new PDFData()
                {
                    Id = generalUUID,
                    FileName = pdfDataName,
                    Created = DateTime.UtcNow,
                    Name = pdfDataName
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
                return BadRequest($"An error occurred: {ex.Message} {ex.InnerException}");
            }
        }
    }
}