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

        [HttpPost("{scratchId}/total")]
        public async Task<IActionResult> ExecuteAsync(Guid scratchId, string testName = "test1", int totalQuestions = 20, bool verifyQuestion = false)
        {
            if (string.IsNullOrWhiteSpace(testName))
                return BadRequest("Test name is required.");

            try
            {
                var scratch = await _scratchRepository.GetScratchByIdAsync(scratchId);
                if (scratch == null)
                    throw new Exception($"Scratch with ID {scratchId} not found.");

                if (scratch.Drafts == null || !scratch.Drafts.Any())
                    throw new Exception("No drafts available.");

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    Name = testName,
                    Questions = new List<Question>(),
                    Created = DateTime.UtcNow
                };

                var drafts = scratch.Drafts.ToList();
                int draftCount = drafts.Count;

                for (int i = 0; i < totalQuestions; i++)
                {
                    int currentDraftIndex = i % draftCount;
                    var draft = drafts[currentDraftIndex];
                    var question = draft.GetRandomQuestions(1).FirstOrDefault();

                    if (question != null)
                    {
                        if (!verifyQuestion || question.Verified)
                        {
                            test.Questions.Add(question);
                        }
                    }
                }

                if (test.Questions.Count < totalQuestions)
                {
                    return BadRequest($"Total questions collected: {test.Questions.Count}. {totalQuestions} are required.");
                }

                await _testRepository.CreateTestAsync(test);
                return Ok(test);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating test from draft: {ex.Message}");
            }
        }

        [HttpPost("{scratchId}/allquestions")]
        public async Task<IActionResult> ExecuteAsync(Guid scratchId, string testName = "Teste1", bool verifyQuestion = false)
        {
            try
            {
                var scratch = await _scratchRepository.GetScratchByIdAsync(scratchId);
                if (scratch == null)
                    throw new Exception($"Scratch with ID {scratchId} not found.");

                if (scratch.Drafts == null || !scratch.Drafts.Any())
                    throw new Exception("No drafts available.");

                var allQuestions = new List<Question>();

                foreach (var draft in scratch.Drafts)
                {
                    var questions = draft!.Questions!.ToList();
                    if (verifyQuestion)
                    {
                        questions = questions.Where(q => q.Verified).ToList();
                    }
                    allQuestions.AddRange(questions);
                }

                if (!allQuestions.Any())
                {
                    return BadRequest("No questions available.");
                }

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    Name = testName,
                    Questions = allQuestions,
                    Created = DateTime.UtcNow
                };

                await _testRepository.CreateTestAsync(test);
                return Ok(test);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating test with all questions: {ex.Message}");
            }
        }
    }
}
