using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.API.Controllers.CourseControllers.QuestionControllers;
using BrunoTheBot.DataContext.Repositories;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FromPDFToJsonStream : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CreateQuestionsFromBookController _createQuestionController;
        private readonly AILogRepository _aILogRepository;

        public FromPDFToJsonStream(
            IHttpClientFactory httpClientFactory,
            CreateQuestionsFromBookController createQuestionsController,
            AILogRepository aILogRepository
            )
        {
            _httpClientFactory = httpClientFactory;
            _createQuestionController = createQuestionsController;
            _aILogRepository = aILogRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] string filePath = "bucket/pdf-files/gc.pdf")
        {
            var generalUUID = Guid.NewGuid();

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

                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5090/extract-text-stream") { Content = content };
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(line) && line.StartsWith("data:"))
                    {
                        var text = line.Substring(5);
                        Console.WriteLine("Received text part: " + text);

                    }
                }

                return Ok("Processing complete");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}