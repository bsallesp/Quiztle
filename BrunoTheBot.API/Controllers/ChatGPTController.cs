using BrunoTheBot.APIs;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTController : ControllerBase
    {
        private readonly ChatGPTAPI _chatGPTAPI;

        public ChatGPTController(ChatGPTAPI chatGPTAPI)
        {
            _chatGPTAPI = chatGPTAPI;
        }

        [HttpPost("chat")]
        public async Task<ActionResult<string>> ChatWithGPT([FromBody] string input)
        {
            try
            {
                // Chama o método ChatWithGPT da classe ChatGPTAPI
                var response = await _chatGPTAPI.ChatWithGPT(input);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Se ocorrer um erro, retorna uma resposta de erro com o status 500
                return StatusCode(500, $"Erro ao interagir com a API ChatGPT: {ex.Message}");
            }
        }
    }
}
