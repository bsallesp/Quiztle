using Quiztle.CoreBusiness.Entities.PDFData;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.DataContext.Repositories.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.Tests
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetTestByIdController : ControllerBase
    {
        private readonly TestRepository _testRepository;

        public GetTestByIdController(TestRepository testRepository)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> ExecuteAsync(Guid id)
        {
            var response = await _testRepository.GetTestByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}