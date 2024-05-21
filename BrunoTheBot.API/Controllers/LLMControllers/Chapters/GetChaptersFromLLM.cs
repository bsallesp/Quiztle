using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.LLMControllers
{
    public class GetChaptersFromLLM(IChatGPTRequest chatGPTAPI, SaveAILogController fromLLMToLogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly SaveAILogController _fromLLMToLogController = fromLLMToLogController;

        public async Task<ActionResult<APIResponse<List<Chapter>>>> ExecuteAsync(string book, int chapterAmount = 5)
        {
            try
            {
                var prompt = LLMPrompts.GetNewChaptersFromBookPrompt(book, chapterAmount);
                var responseLLM = await _chatGPTRequest.ExecuteAsync(prompt) ?? throw new Exception();
                await _fromLLMToLogController.ExecuteAsync(nameof(ExecuteAsync), responseLLM);
                var newChapters = JSONConverter.ConvertToChapters(responseLLM, "NewChapters");
                if (newChapters.Count <= 0 || newChapters == null) throw new Exception("The ChaptersResponseAPILLM amount is zero or null");

                APIResponse<List<Chapter>> chaptersResponseAPILLM = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = newChapters
                };

                return chaptersResponseAPILLM ?? throw new Exception("chaptersResponseAPILLM is null");
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
