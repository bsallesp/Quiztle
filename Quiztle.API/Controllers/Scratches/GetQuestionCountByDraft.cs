using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.DataService.Repository;

namespace Quiztle.API.Controllers.ScratchControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetQuestionsCountByDraft : ControllerBase
    {
        private readonly ScratchRepository _scratchrepository;

        public GetQuestionsCountByDraft(ScratchRepository scratchrepository)
        {
            _scratchrepository = scratchrepository;
        }
        
        [HttpPost()]
        public async Task<IActionResult> ExecuteAsync()
        {
            try
            {
                var scratches = await _scratchrepository.GetAllScratchesAsync();

                foreach (var scratch in scratches)
                {
                    foreach (var draft in scratch!.Drafts!)
                    {
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine(draft.Id);
                        Console.WriteLine(draft.Questions!.Count);
                        Console.WriteLine("--------------------------------------------");
                    }
                }

                return Ok(scratches);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}