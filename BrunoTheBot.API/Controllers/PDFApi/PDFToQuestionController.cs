using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;
using BrunoTheBot.API.Controllers.CourseControllers.QuestionControllers;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFToQuestionController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CreateQuestionsFromBookController _createQuestionController;

        public PDFToQuestionController(IHttpClientFactory httpClientFactory, CreateQuestionsFromBookController createQuestionsController)
        {
            _httpClientFactory = httpClientFactory;
            _createQuestionController = createQuestionsController;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] string filePath = "bucket/pdf-files/gc.pdf")
        {
            if (string.IsNullOrEmpty(filePath))
                return BadRequest("File path not provided");

            var client = _httpClientFactory.CreateClient();

            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(filePath), "file_path");
                content.Add(new StringContent("1"), "partial_output_rate");

                var response = await client.PostAsync("http://localhost:5090/extract-text", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                foreach (var item in GetTextSplittedByTokens(result))
                {
                    //_createQuestionController.ExecuteAsync()
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<string> GetTextSplittedByTokens(string jsonInput)
        {
            try
            {
                PdfData pdfData = JsonSerializer.Deserialize<PdfData>(jsonInput) ?? throw new Exception("jsonInput eh nulo");

                return pdfData.ExtractedTextParts ?? throw new Exception("ExtractedTextParts eh nulo");
            }
            catch
            {
                return new List<string>();
            }
        }

        public class PdfData
        {
            public string? OriginalFilename { get; set; }
            public List<string>? ExtractedTextParts { get; set; }
            public string? TimeTaken { get; set; }
            public string? MemoryUsed { get; set; }
        }
    }
}
