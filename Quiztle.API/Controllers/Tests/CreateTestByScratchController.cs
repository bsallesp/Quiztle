using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.DataService.Repository.Quiz;

namespace Quiztle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTestByScratchController : ControllerBase
    {
        private readonly ScratchRepository _scratchRepository;
        private readonly TestRepository _testRepository;

        public CreateTestByScratchController(ScratchRepository scratchRepository,
                                             TestRepository testRepository)
        {
            _scratchRepository = scratchRepository;
            _testRepository = testRepository;
        }

        [HttpPost("{scratchId}")]
        public async Task<IActionResult> ExecuteAsync(Guid scratchId, string testName, int questionsPerScratch)
        {
            if (testName is not null)
            {
                try
                {
                    var scratch = await _scratchRepository.GetScratchByIdAsync(scratchId);

                    if (scratch == null) return NotFound($"Scratch with ID {scratchId} not found.");
                    if (scratch.Drafts == null) return NotFound($"Drafts not found.");
                    if (scratch.Drafts.Select(q => q.Questions) == null) return NotFound($"No questions found.");

                    var questions = new List<Question>();

                    foreach (var item in scratch.Drafts) questions.AddRange([.. item.GetRandomQuestions(questionsPerScratch)]);

                    questions = questions.Where(v => v.Verified).ToList();

                    Test test = new()
                    {
                        Id = Guid.NewGuid(),
                        Name = scratch.Name!,
                        Questions = questions,
                        Created = DateTime.UtcNow
                    };

                    await _scratchRepository.SaveChangesAsync(test);

                    return Ok(test);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error creating test from draft: {ex.Message}");
                }
            }

            throw new ArgumentNullException(nameof(testName));
        }
    }
}
