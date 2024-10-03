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

                return Ok(allScratchesFiltered);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating test from scratch: {ex.Message}");
            }
        }
    }
}
