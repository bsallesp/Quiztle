using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.API.Controllers.CourseControllers.QuestionControllers;
using BrunoTheBot.DataContext.Repositories;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.CoreBusiness.Entities.PDFData;
using static System.Net.Mime.MediaTypeNames;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FromPDFToJsonStream : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CreateQuestionsFromBookController _createQuestionController;
        private readonly AILogRepository _aILogRepository;
        private readonly PDFDataRepository _pDFDataRepository;

        public FromPDFToJsonStream(
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
        public async Task<IActionResult> ExecuteAsync([FromBody] string filePath = "bucket/pdf-files/gc.pdf")
        {
            var generalUUID = Guid.NewGuid();

            var newPDFData = new PDFData()
            {
                Id = generalUUID,
                FileName = Path.GetFileName(filePath),
                Created = DateTime.UtcNow
            };

            if (string.IsNullOrEmpty(filePath))
                return BadRequest("File path not provided");

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromDays(1);

            try
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(filePath), "file_path" },
                    { new StringContent("1"), "partial_output_rate" }
                };

                //var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5090/extract-text-stream") { Content = content };
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5090/extract-text-mupdf") { Content = content };

                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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