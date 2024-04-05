using BrunoTheBot.API.Controllers.FromLLMControllers;
using BrunoTheBot.API.Controllers.StaticsStatusCodes;
using BrunoTheBot.API.Prompts;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.FromLLMToDBControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicClassesFromLLM(IChatGPTRequest chatGPTAPI, FromLLMToLogController fromLLMToLogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly FromLLMToLogController _fromLLMToLogController = fromLLMToLogController;

        [HttpPost("GetNewTopicClassesFromLLM")]
        public async Task<ActionResult<string>> GetNewTopicClassesFromLLM([FromBody] string school, int subTopicsClassesAmount = 10)
        {
            try
            {
                var response = await _chatGPTRequest.ChatWithGPT(LLMPrompts.GetTopicsClassesToSchoolPrompt(school));
                
                await _fromLLMToLogController.SaveLog(nameof(GetNewTopicClassesFromLLM), response);
                
                var responseObject = new
                {
                    status = CustomStatusCodes.SuccessStatus,
                    response
                };

                return Ok(responseObject);

            }
            catch (Exception ex)
            {
                var responseObject = new
                {
                    status = CustomStatusCodes.SuccessStatus,
                    response = ex.Message
                };

                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }
    }
}
