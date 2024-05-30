using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.DataContext.DataService.Repository.Quiz;
using BrunoTheBot.API.Services;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTestFromPDFDataPages : ControllerBase
    {
        private readonly PDFDataRepository _pDFDataRepository;
        private readonly TestRepository _testRepository;
        private readonly GetQuestionsFromLLM _getQuestionsFromLLM;
        private readonly QuestionRepository _questionRespository;

        public CreateTestFromPDFDataPages(PDFDataRepository pDFDataRepository,
            QuestionRepository questionRepository,
            TestRepository testRepository,
            GetQuestionsFromLLM getQuestionsFromLLM)
        {
            _pDFDataRepository = pDFDataRepository;
            _testRepository = testRepository;
            _getQuestionsFromLLM = getQuestionsFromLLM;
            _questionRespository = questionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteAsync(Guid id, string name, int startPage, int endPage)
        {
            try
            {
                var pdfData = await _pDFDataRepository.GetPDFDataByIdAsyncByPage(id, startPage, endPage);
                if (pdfData == null)
                    return NotFound("PDF data not found with this Id.");

                var pages = pdfData.Pages.Select(p => p.Content).ToList();
                var pagesConcat = string.Join("\n", pages);

                var dividedPages = OpenAITokenManager.SplitTextIntoTokenSafeParts(pagesConcat, 3000);

                List<Question> questions = [];

                var testGuid = Guid.NewGuid();

                foreach (var item in dividedPages)
                {
                    try
                    {
                        var newQuestions = await _getQuestionsFromLLM.ExecuteAsync(item, 5);
                        if (newQuestions.Value == null) continue;

                        questions.AddRange(newQuestions.Value.Data);
                    }
                    catch
                    {
                        continue;
                    }
                }

                Test test = new Test
                {
                    Id = testGuid,
                    Name = name,
                    Questions = questions,
                    PDFDataId = id
                };

                await _testRepository.CreateTestAsync(test);

                return Ok(test);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}