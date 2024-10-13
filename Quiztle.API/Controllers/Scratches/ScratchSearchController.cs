using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.CoreBusiness.Entities.Scratch;

namespace Quiztle.API.Controllers.ScratchControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScratchSearchController : ControllerBase
    {
        private readonly ScratchRepository _scratchRepository;

        public ScratchSearchController(ScratchRepository scratchRepository)
        {
            _scratchRepository = scratchRepository;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchScratchesByKeywordsAsync([FromQuery] string[] keywords)
        {
            try
            {
                // Chama o método de busca com as palavras-chave fornecidas
                var scratches = await _scratchRepository.SearchScratchesAndDraftsByKeywordsAsync(keywords);

                // Se não houver scratches encontrados, retorna 404
                if (scratches == null || !scratches.Any())
                {
                    return NotFound("No scratches found matching the provided keywords.");
                }

                // Retorna os scratches encontrados com sucesso
                return Ok(scratches);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
