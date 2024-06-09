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
        #region CTOR and others

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

        #endregion

        [HttpGet]
        public async Task<IActionResult> ExecuteAsync([FromQuery] string fileName, [FromQuery] string pdfDataName, [FromQuery] int partialOutputRate = 1)
        {
            try
            {
                Console.WriteLine("Starting to extract...");

                var client = _httpClientFactory.CreateClient("PDFClient");
                var generalUUID = Guid.NewGuid();

                var newPDFData = new PDFData()
                {
                    Id = generalUUID,
                    FileName = pdfDataName,
                    Created = DateTime.UtcNow,
                    Name = pdfDataName
                };

                string requestUri = $"extract-text-mupdf/{Uri.EscapeDataString(fileName)}/{partialOutputRate}";
                Console.WriteLine($"Final request URL: {new Uri(client.BaseAddress!, requestUri)}");

                using var response = await client.GetAsync(new Uri(client.BaseAddress!, requestUri));
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