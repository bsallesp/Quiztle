using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.DataContext.DataService.Repository.Quiz;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTestFromPDFDataPages : ControllerBase
    {
        private readonly PDFDataRepository _pDFDataRepository;
        private readonly TestRepository _testRepository;
        private readonly GetQuestionsFromLLM _getQuestionsFromLLM;

        public CreateTestFromPDFDataPages(PDFDataRepository pDFDataRepository, TestRepository testRepository, GetQuestionsFromLLM getQuestionsFromLLM)
        {
            _pDFDataRepository = pDFDataRepository;
            _testRepository = testRepository;
            _getQuestionsFromLLM = getQuestionsFromLLM;
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteAsync(Guid id, int startPage, int endPage)
        {
            try
            {
                var pdfData = await _pDFDataRepository.GetPDFDataByIdAsyncByPage(id, startPage, endPage);
                if (pdfData == null)
                    return NotFound("PDF data not found with this Id.");

                var pages = pdfData.Pages.Select(p => p.Content).ToList();
                var pagesConcat = string.Join("\n", pages);

                var result = await _getQuestionsFromLLM.ExecuteAsync(pagesConcat, 5);
                if (result.Value == null) return NotFound("_getQuestionsFromLLM.ExecuteAsync nao retornou valores");

                Test test = new Test {
                    Questions = result.Value.Data,
                    PDFDataId = id
                };
                await _testRepository.CreateTestAsync(test);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}