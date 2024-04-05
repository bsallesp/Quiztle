using BrunoTheBot.API.Prompts;
using BrunoTheBot.CoreBusiness.Log;
using BrunoTheBot.DataContext.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BrunoTheBot.API.Controllers.StaticsStatusCodes;

namespace BrunoTheBot.API.Controllers.FromLLMControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FromLLMToLogController : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest;
        private readonly AILogRepository aiLogDb;


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
                var response = await _chatGPTRequest.ChatWithGPT(LLMPrompts.SearchSchoolPrompt(input));
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
                var response = await _chatGPTRequest.ChatWithGPT(LLMPrompts.SearchLearningContentPrompt(input));

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
                var response = await _chatGPTRequest.ChatWithGPT(LLMPrompts.GetTopics(input, amount));

                await SaveLog(input, response);

                var responseObject = new
                {
                    status = CustomStatusCodes.SuccessStatus,
                    response
                };

                return JsonConvert.SerializeObject(responseObject);

            }
            catch (Exception ex)
            {
                var responseObject = new
                {
                    status = CustomStatusCodes.ErrorStatus,
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
                var response = await _chatGPTRequest.ChatWithGPT(LLMPrompts.GetBestAuthors(input));

                await SaveLog(input, response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao interagir com a API ChatGPT: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task SaveLog(string name, string json)
        {
            await aiLogDb.CreateAILogAsync(new AILog
            {
                Name = name,
                JSON = json
            });
        }
    }
}