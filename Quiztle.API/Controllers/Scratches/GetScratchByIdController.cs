using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository;

namespace Quiztle.API.Controllers.ScratchControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetScratchByIdController : ControllerBase
    {
        private readonly ScratchRepository _scratchRepository;

        public GetScratchByIdController(ScratchRepository scratchRepository)
        {
            _scratchRepository = scratchRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScratchByIdAsync(Guid id)
        {
            try
            {
                var scratch = await _scratchRepository.GetScratchByIdAsync(id);

                if (scratch == null)
                {
                    return NotFound($"Scratch with ID {id} not found.");
                }

                return Ok(scratch);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
