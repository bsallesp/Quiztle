using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.DataService.Repository.Quiz;

namespace Quiztle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTestByDraftController : ControllerBase
    {
        private readonly DraftRepository _draftRepository;
        private readonly TestRepository _testRepository;

        public CreateTestByDraftController(DraftRepository draftRepository, TestRepository testRepository)
        {
            _draftRepository = draftRepository;
            _testRepository = testRepository;
        }

        [HttpPost("{draftId}")]
        public async Task<IActionResult> ExecuteAsync(Guid draftId, string testName)
        {
            try
            {
                var draft = await _draftRepository.GetDraftWithQuestionsAsync(draftId, trackChanges: true);

                if (draft == null)
                {
                    return NotFound($"Draft with ID {draftId} not found.");
                }

                if (draft.Questions == null || draft.Questions.Count == 0)
                {
                    return BadRequest("The draft has no questions to create a test.");
                }

                Test test = new()
                {
                    Id = Guid.NewGuid(),
                    Name = testName,
                    Questions = draft.Questions,
                    Created = DateTime.UtcNow
                };

                await _testRepository.CreateTestAsync(test);


                return Ok(test);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating test from draft: {ex.Message}");
            }
        }
    }
}
