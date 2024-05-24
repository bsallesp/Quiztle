using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.API.Controllers.LLMControllers;
using System.Linq;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetQuestionsController : ControllerBase
    {
        private readonly PDFDataRepository _pDFDataRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly GetQuestionsFromLLM _getQuestionsFromLLM;

        public GetQuestionsController(PDFDataRepository pDFDataRepository, QuestionRepository questionRepository, GetQuestionsFromLLM getQuestionsFromLLM)
        {
            _pDFDataRepository = pDFDataRepository;
            _questionRepository = questionRepository;
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

                foreach (var item in result.Value.Data) await _questionRepository.CreateQuestionAsync(item);

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}