using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.CoreBusiness.Entities.Scratch;

namespace Quiztle.API.Controllers.ScratchControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllScratchesController : ControllerBase
    {
        private readonly ScratchRepository _scratchRepository;

        public GetAllScratchesController(ScratchRepository scratchRepository)
        {
            _scratchRepository = scratchRepository;
        }

        // Método para obter todos os Scratches com Drafts, Questions e Options
        [HttpGet("all")]
        public async Task<IActionResult> GetAllScratchesAsync()
        {
            try
            {
                var scratches = await _scratchRepository.GetAllScratchesAsync();

                // Se não houver scratches, retorna 404
                if (scratches == null || !scratches.Any())
                {
                    return NotFound("No scratches found.");
                }

                // Retorna todos os scratches encontrados
                return Ok(scratches);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
