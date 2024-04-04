using BrunoTheBot.API.Prompts;
using BrunoTheBot.CoreBusiness.Log;
using BrunoTheBot.DataContext.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BrunoTheBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FromLLMToLogController : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest;
        private readonly AILogRepository aiLogDb;
        
        private const string SuccessStatus = "Success";
        private const string ErrorStatus = "Error";

        public FromLLMToLogController(IChatGPTRequest chatGPTAPI, AILogRepository aiLogDb)
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
                var response = await _chatGPTRequest.ChatWithGPT(CustomPromptsToRequest.GetTopics(input, amount));

                await SaveLog(input, response);

                var responseObject = new
                {
                    status = SuccessStatus,
                    response
                };

                return JsonConvert.SerializeObject(responseObject);

            }
            catch (Exception ex)
            {
                var responseObject = new
                {
                    status = ErrorStatus,
                    response = ex.Message
                };

                return JsonConvert.SerializeObject(responseObject);
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
            await aiLogDb.CreateAILogAsync(new AILog
            {
                Name = name,
                JSON = json
            });
        }
    }
}