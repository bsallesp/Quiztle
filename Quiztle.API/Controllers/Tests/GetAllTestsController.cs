using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.Tests
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetAllTestsController : ControllerBase
    {
        private readonly TestRepository _testRepository;

        public GetAllTestsController(TestRepository testRepository)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
        }

        [HttpGet("alltests")]
        public async Task<ActionResult<List<Test>>> GetAllTestsAsync()
        {
            var response = await _testRepository.GetAllTestsAsync();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        
        [HttpGet("alltests/includequestions/{includeQuestions}")]
        public async Task<ActionResult<List<Test>>> GetAllTestsWithQuestionsAsync(bool includeQuestions = false)
        {
            var response = await _testRepository.GetAllTestsAsync(includeQuestions);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}