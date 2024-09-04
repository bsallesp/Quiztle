using Microsoft.AspNetCore.Mvc;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.CoreBusiness.Utils;

namespace Quiztle.API.Controllers.LLM
{
    namespace Quiztle.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class OllamaTestController : ControllerBase
        {
            private readonly ILLMRequest _ollamaRequest;
            
            public OllamaTestController(ILLMRequest ollamaRequest)
            {
                _ollamaRequest = ollamaRequest;
            }

            [HttpPost("execute")]
            public async Task<IActionResult> ExecuteAsync([FromBody] string input = "retorne um json com 1 pergunta e 5 respostas, sendo 1 resposta certa, sobre microsoft AZ-900. nao retorne nenhum texto alem do json, preciso do json puro para que minha API nao tenha nenhhum erro.")
            {
                try
                {
                    var response = await _ollamaRequest.ExecuteAsync(input);
                    var jsonResponse = JsonExtractor.ExtractFromLLMResponse(response);
                    Console.WriteLine(response);
                    Console.WriteLine(jsonResponse);
                    return Ok(jsonResponse);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro in OllamaTestController: {ex.Message}");
                }
            }
        }
    }
}