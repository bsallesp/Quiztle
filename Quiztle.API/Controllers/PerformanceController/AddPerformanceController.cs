using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Performance;
using Quiztle.DataContext.DataService.Repository.Performance;

namespace Quiztle.API.Controllers.PerformanceController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddPerformanceController : ControllerBase
    {
        private readonly TestPerformanceRepository _testPerformanceRepository;

        public AddPerformanceController(TestPerformanceRepository testPerformanceRepository)
        {
            _testPerformanceRepository = testPerformanceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] TestPerformance testPerformance)
        {
            try
            {
                var performanceGuid = Guid.NewGuid();

                TestPerformance newTestPerformance = new()
                {
                    Id = performanceGuid,
                    UserId = testPerformance.UserId,
                    TestName = testPerformance.TestName,
                    QuestionsPerformance = testPerformance.QuestionsPerformance,
                    CorrectAnswers = testPerformance.CorrectAnswers,
                    IncorrectAnswers = testPerformance.IncorrectAnswers,
                    Score = testPerformance.Score
                };

                await _testPerformanceRepository.CreateTestPerformanceAsync(newTestPerformance);

                return Ok(newTestPerformance);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
