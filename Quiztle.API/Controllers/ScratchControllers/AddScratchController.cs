using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Scratch;
using Quiztle.DataContext.DataService.Repository;


namespace Quiztle.API.Controllers.ScratchControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddScratchController : ControllerBase
    {
        private readonly ScratchRepository _scratchrepository;

        public AddScratchController(ScratchRepository scratchrepository)
        {
            _scratchrepository = scratchrepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] Scratch scratch)
        {
            try
            {
                var scratchGuid = Guid.NewGuid();

                Scratch newScratch = new()
                {
                    Id = scratchGuid,
                    Name = scratch.Name,
                    Drafts = scratch.Drafts,
                    Created = DateTime.UtcNow
                };

                await _scratchrepository.CreateTestAsync(newScratch);

                return Ok(newScratch);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}