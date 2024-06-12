using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Entities.PDFData;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.DataContext.Repositories.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.Tests
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemoveTestController : ControllerBase
    {
        private readonly TestRepository _testRepository;

        public RemoveTestController(TestRepository testRepository)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> ExecuteAsync(Guid id)
        {
            var response = await _testRepository.RemoveTestById(id);
            if (response == false)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}