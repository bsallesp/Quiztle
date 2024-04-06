using BrunoTheBot.API.Controllers.FromLLMControllers;
using BrunoTheBot.API.Controllers.StaticsStatusCodes;
using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
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
        public async Task<ActionResult<TopicClassesAPIResponse>> GetNewTopicClassesFromLLM([FromBody] string school, int subTopicsClassesAmount = 10)
        {
            try
            {
                var responseLLM = await _chatGPTRequest.ChatWithGPT(LLMPrompts.GetTopicsClassesToSchoolPrompt(school)) ?? throw new Exception();
                
                await _fromLLMToLogController.SaveLog(nameof(GetNewTopicClassesFromLLM), responseLLM);

                TopicClassesAPIResponse TopicClassesResponseAPILLM = new TopicClassesAPIResponse
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    TopicClasses = ConvertJSONToObjects.ConvertToTopicClasses(responseLLM)
                };

                return Ok(TopicClassesResponseAPILLM ?? throw new Exception());

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
