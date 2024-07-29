using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.Repositories.Quiz;
using Quiztle.API.Controllers.LLMControllers;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.API.Services;

namespace Quiztle.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTestController : ControllerBase
    {
        private readonly TestRepository _testRepository;
        private readonly QuestionRepository _questionRespository;

        public CreateTestController(PDFDataRepository pDFDataRepository,
            QuestionRepository questionRepository,
            TestRepository testRepository,
            GetQuestionsFromLLM getQuestionsFromLLM)
        {
            _testRepository = testRepository;
            _questionRespository = questionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] Test newTest)
        {
            try
            {
                var testGuid = Guid.NewGuid();

                Test test = new()
                {
                    Id = testGuid,
                    Name = newTest.Name,
                    Questions = newTest.Questions,
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
