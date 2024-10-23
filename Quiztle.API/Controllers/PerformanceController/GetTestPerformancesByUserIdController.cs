using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository.Performance;

namespace Quiztle.API.Controllers.PerformanceController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTestPerformancesByUserIdController : ControllerBase
    {
        private readonly TestPerformanceRepository _testPerformanceRepository;

        public GetTestPerformancesByUserIdController(TestPerformanceRepository testPerformanceRepository)
        {
            _testPerformanceRepository = testPerformanceRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ExecuteAsync(Guid userId)
        {
            try
            {
                var performances = await _testPerformanceRepository.GetTestPerformancesByUserAsync(userId);

                return Ok(performances);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
