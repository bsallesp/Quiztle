using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.DataService.Repository.Quiz;
using System.Collections.Immutable;

namespace Quiztle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAllTestsByScratchController : ControllerBase
    {
        private readonly ScratchRepository _scratchRepository;
        private readonly TestRepository _testRepository;
        private readonly CreateTestByDraftController _createTestByDraftController;

        public CreateAllTestsByScratchController(
            ScratchRepository scratchRepository,
            TestRepository testRepository,
            CreateTestByDraftController createTestByDraftController
            )
        {
            _scratchRepository = scratchRepository;
            _testRepository = testRepository;
            _createTestByDraftController = createTestByDraftController;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync()
        {
            try
            {
                var allScratchesFiltered = await _scratchRepository.GetFilteredScratchesAsync();
                if (allScratchesFiltered == null) return NotFound();

                //var result = _testRepository.CreateTestsFromScratchesAsync(allScratchesFiltered.ToImmutableList(), 10);



                //    .GetDraftWithQuestionsAsync(scratchId, trackChanges: true);

                //if (scratch == null)
                //{
                //    return NotFound($"Draft with ID {scratchId} not found.");
                //}

                //if (scratch.Questions == null || !scratch.Questions.Any())
                //{
                //    return BadRequest("The scratch has no questions to create a test.");
                //}

                //// Cria um novo Test com as perguntas do Draft
                //Test test = new()
                //{
                //    Id = Guid.NewGuid(),
                //    Name = testName,
                //    Questions = scratch.Questions,
                //    Created = DateTime.UtcNow
                //};

                //// Adiciona o Test ao DbContext
                //await _testRepository.CreateTestAsync(test);


                return Ok(allScratchesFiltered);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating test from scratch: {ex.Message}");
            }
        }
    }
}
