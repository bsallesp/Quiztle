using Microsoft.AspNetCore.Mvc;
using Quiztle.API.Controllers.LLMControllers;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.CoreBusiness.Entities.Scratch;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.DataContext.Repositories.Quiz;

namespace Quiztle.API.Controllers.Tests
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTestsByListScratchesIdsController : ControllerBase
    {
        private readonly TestRepository _testRepository;
        private readonly ScratchRepository _scratchRepository;

        public CreateTestsByListScratchesIdsController(TestRepository testRepository, ScratchRepository scratchRepository)
        {
            _testRepository = testRepository;
            _scratchRepository = scratchRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] List<Guid> guids, string testName)
        {
            try
            {
                var newTest = new Test
                {
                    Name = testName,
                    Questions = [],
                };


                foreach (var guid in guids)
                {
                    var scratch = await _scratchRepository.GetScratchByIdAsync(guid);
                    foreach (var draft in scratch!.Drafts!) newTest.Questions.AddRange(draft.GetRandomQuestions(10));
                }

                await _scratchRepository.SaveChangesAsync(newTest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}