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

        public CreateTestFromPDFDataPages(PDFDataRepository pDFDataRepository,
            TestRepository testRepository,
            GetQuestionsFromLLM getQuestionsFromLLM)
        {
            _pDFDataRepository = pDFDataRepository;
            _testRepository = testRepository;
            _getQuestionsFromLLM = getQuestionsFromLLM;
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
                Test test = new Test
                {
                    Id = new Guid(),
                    Name = name,
                    Questions = [],
                    PDFDataId = id
                };

                await _testRepository.CreateTestAsync(test);

                foreach (var item in dividedPages)
                {
                    //Thread.Sleep(1000);

                    try
                    {
                        var newQuestions = await _getQuestionsFromLLM.ExecuteAsync(item, 3);
                        if (newQuestions.Value == null) continue;
                        await _testRepository.AddQuestionsToTestAsync(test.Id, newQuestions.Value.Data);
                        Console.WriteLine(test.Questions.Count);
                    }
                    catch
                    {
                        continue;
                    }

                }

                return Ok(test);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}