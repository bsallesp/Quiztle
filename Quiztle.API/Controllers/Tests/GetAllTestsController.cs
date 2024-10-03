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

        [HttpGet]
        public async Task<ActionResult<List<Test>>> ExecuteAsync()
        {
            var response = await _testRepository.GetAllTestsAsync();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}