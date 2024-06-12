using Microsoft.AspNetCore.Mvc;
using Quiztle.API.Controllers.CourseControllers.QuestionControllers;
using Quiztle.DataContext.Repositories;
using Quiztle.CoreBusiness.Entities.PDFData;
using System.IO;
using Quiztle.DataContext.Repositories.Quiz;

namespace Quiztle.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePDFDataFromStreamController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PDFDataRepository _pdfDataRepository;

        public CreatePDFDataFromStreamController(
            IHttpClientFactory httpClientFactory,
            PDFDataRepository pdfDataRepository)
        {
            _httpClientFactory = httpClientFactory;
            _pdfDataRepository = pdfDataRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteAsync([FromQuery] string fileName, [FromQuery] string pdfDataName, [FromQuery] int partialOutputRate = 1)
        {
            try
            {
                Console.WriteLine("Starting to extract...");
                var client = CreateHttpClient("PDFClient");
                var newPDFData = InitializePDFData(pdfDataName);

                string requestUri = FormatRequestUri(fileName, partialOutputRate);
                using var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                await ProcessPDFStream(response, newPDFData);

                await _pdfDataRepository.CreatePDFDataAsync(newPDFData);

                return Ok("Processing complete");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message} {ex.InnerException}");
            }
        }

        private HttpClient CreateHttpClient(string clientName)
        {
            return _httpClientFactory.CreateClient(clientName);
        }

        private static PDFData InitializePDFData(string pdfDataName)
        {
            return new PDFData
            {
                Id = Guid.NewGuid(),
                FileName = pdfDataName,
                Created = DateTime.UtcNow,
                Name = pdfDataName
            };
        }

        private string FormatRequestUri(string fileName, int partialOutputRate)
        {
            return $"extract-text-mupdf/{Uri.EscapeDataString(fileName)}/{partialOutputRate}";
        }

        private async Task ProcessPDFStream(HttpResponseMessage response, PDFData pdfData)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            var count = 0;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (IsDataLine(line))
                {
                    var text = ExtractTextFromDataLine(line);
                    LogReceivedText(text);
                    pdfData.Pages.Add(new PDFDataPages
                    {
                        Content = text,
                        Page = ++count,
                        Created = DateTime.UtcNow
                    });
                }
            }
        }

        private static bool IsDataLine(string line)
        {
            return !string.IsNullOrEmpty(line) && line.StartsWith("data:");
        }

        private static string ExtractTextFromDataLine(string line)
        {
            return line.Substring(5);
        }

        private static void LogReceivedText(string text)
        {
            Console.WriteLine("Received text part: " + text);
        }
    }
}
