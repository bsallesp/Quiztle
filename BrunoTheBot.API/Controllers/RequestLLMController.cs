using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestLLMController : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest;
        private readonly DataContext.AILogRepository aiLogDb;

        public RequestLLMController(IChatGPTRequest chatGPTAPI, DataContext.AILogRepository aiLogDb)
        {
            _chatGPTRequest = chatGPTAPI;
            this.aiLogDb = aiLogDb;
        }

        [HttpPost("GetSchoolFeatures")]
        public async Task<ActionResult<string>> GetSchoolFeatures([FromBody] string input)
        {
            try
            {
                var response = await _chatGPTRequest.ChatWithGPT(CustomPromptsToRequest.SearchSchoolPrompt(input));
                await SaveLog(input, response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao interagir com a API ChatGPT: {ex.Message}");
            }
        }

        [HttpPost("GetSchoolProbaPotential")]
        public async Task<ActionResult<string>> GetSchoolProbaPotential([FromBody] string input)
        {
            try
            {
                var response = await _chatGPTRequest.ChatWithGPT(CustomPromptsToRequest.SearchLearningContentPrompt(input));

                await SaveLog(input, response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao interagir com a API ChatGPT: {ex.Message}");
            }
        }

        [HttpPost("GetSubTopics")]
        public async Task<ActionResult<string>> GetSubTopics([FromBody] string input, int amount)
        {
            try
            {
                var response = await _chatGPTRequest.ChatWithGPT(CustomPromptsToRequest.GetSubTopics(input, amount));

                await SaveLog(input, response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao interagir com a API ChatGPT: {ex.Message}");
            }
        }


        [HttpPost("GetBestAuthors")]
        public async Task<ActionResult<string>> GetBestAuthors([FromBody] string input)
        {
            try
            {
                var response = await _chatGPTRequest.ChatWithGPT(CustomPromptsToRequest.GetBestAuthors(input));

                await SaveLog(input, response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao interagir com a API ChatGPT: {ex.Message}");
            }
        }

        private async Task SaveLog(string name, string json)
        {
            await aiLogDb.CreateAILogAsync(new CoreBusiness.AILog
            {
                Name = name,
                JSON = json
            });
        }
    }
}