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

        [HttpPost("{scratchId}/total")]
        public async Task<IActionResult> ExecuteAsync(Guid scratchId, string testName, int totalQuestions, bool distributeEvenly)
        {
            if (string.IsNullOrWhiteSpace(testName))
                return BadRequest("Test name is required.");

            try
            {
                var scratch = await _scratchRepository.GetScratchByIdAsync(scratchId);
                if (scratch == null)
                    return NotFound($"Scratch with ID {scratchId} not found.");

                if (scratch.Drafts == null || scratch.Drafts.Count == 0)
                    return NotFound("No drafts available.");

                var totalDrafts = scratch.Drafts.Count;
                var questionsPerDraft = totalQuestions / totalDrafts;
                var remainingQuestions = totalQuestions % totalDrafts;

                var questions = new List<Question>();
                foreach (var draft in scratch.Drafts)
                {
                    var draftQuestions = draft.GetRandomQuestions(questionsPerDraft);

                    // Distribute remaining questions evenly
                    if (remainingQuestions > 0)
                    {
                        draftQuestions.AddRange(draft.GetRandomQuestions(1));
                        remainingQuestions--;
                    }

                    questions.AddRange(draftQuestions);
                }

                // Ensure we do not exceed the total number of questions requested
                while (questions.Count > totalQuestions)
                {
                    questions.RemoveAt(new Random().Next(questions.Count));
                }

                // Ensure all questions are verified
                questions = questions.Where(v => v.Verified).ToList();
                if (questions.Count < totalQuestions)
                {
                    return BadRequest($"Total questions collected: {questions.Count}. {totalQuestions} are required.");
                }

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    Name = testName,
                    Questions = questions,
                    Created = DateTime.UtcNow
                };

                await _testRepository.SaveChangesAsync();
                return Ok(test);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating test from draft: {ex.Message}");
            }
        }
    }
}
