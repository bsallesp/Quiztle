using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.DataService.Repository.Quiz;
using System.Diagnostics;

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

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var scratch = await _scratchRepository.GetScratchByIdAsync(scratchId);
                if (scratch == null)
                    return BadRequest($"Scratch with ID {scratchId} not found.");

                var drafts = scratch.Drafts?.ToArray();
                if (drafts == null || drafts.Length == 0)
                    return BadRequest("No drafts available.");

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    Name = testName,
                    Questions = new List<Question>(),
                    Created = DateTime.UtcNow
                };

                int draftCount = drafts.Length;
                var addedQuestionIds = new HashSet<Guid>();
                int addedQuestions = 0;
                int draftIndex = 0;

                // Continua até atingir o número total de perguntas desejado
                while (addedQuestions < totalQuestions)
                {
                    var draft = drafts[draftIndex % draftCount];  // Circula pelos drafts
                    var question = draft.GetRandomQuestions(1).FirstOrDefault();

                    if (question != null && (!verifyQuestion || question.Verified))
                    {
                        if (!addedQuestionIds.Contains(question.Id))
                        {
                            test.Questions.Add(question);
                            addedQuestionIds.Add(question.Id);
                            addedQuestions++;
                        }
                    }

                    draftIndex++;  // Move para o próximo draft

                    // Verifica se circulamos por todos os drafts e não temos mais perguntas únicas para adicionar
                    if (draftIndex >= draftCount * totalQuestions && addedQuestions < totalQuestions)
                        return BadRequest($"Total questions collected: {addedQuestions}. {totalQuestions} are required.");
                }

                await _testRepository.CreateTestAsync(test);

                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                return Ok(new
                {
                    Test = test,
                    Metrics = new
                    {
                        TotalQuestionsAdded = addedQuestions,
                        TimeTakenMilliseconds = elapsedMilliseconds
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating test from draft: {ex.Message}");
            }
        }

        [HttpPost("{scratchId}/allquestions")]
        public async Task<IActionResult> ExecuteAsync(Guid scratchId, string testName = "Teste1", bool verifyQuestion = false)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var scratch = await _scratchRepository.GetScratchByIdAsync(scratchId);
                if (scratch == null)
                    return BadRequest($"Scratch with ID {scratchId} not found.");

                var allQuestions = scratch.Drafts?
                    .SelectMany(d => verifyQuestion ? d.Questions.Where(q => q.Verified) : d.Questions)
                    .ToList();

                if (allQuestions == null || allQuestions.Count == 0)
                    return BadRequest("No questions available.");

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    Name = testName,
                    Questions = allQuestions.DistinctBy(q => q.Id).ToList(),
                    Created = DateTime.UtcNow
                };

                await _testRepository.CreateTestAsync(test);

                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                return Ok(new
                {
                    Test = test,
                    Metrics = new
                    {
                        TotalQuestions = test.Questions.Count,
                        TimeTakenMilliseconds = elapsedMilliseconds
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating test with all questions: {ex.Message}");
            }
        }
    }
}
