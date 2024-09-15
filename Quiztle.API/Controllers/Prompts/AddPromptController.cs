using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Prompts;
using Quiztle.DataContext.DataService.Repository;

namespace Quiztle.API.Controllers.Prompts
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddPromptController : ControllerBase
    {
        private readonly PromptRepository _promptRepository;

        public AddPromptController(PromptRepository promptRepository)
        {
            _promptRepository = promptRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] Prompt prompt)
        {
            try
            {
                if (prompt == null)
                {
                    return BadRequest("Prompt cannot be null.");
                }

                var promptGuid = Guid.NewGuid();

                Prompt newPrompt = new()
                {
                    Id = promptGuid,
                    Created = DateTime.UtcNow,
                    Items = prompt.Items
                };

                await _promptRepository.CreatePromptAsync(newPrompt);

                return Ok(newPrompt);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}
