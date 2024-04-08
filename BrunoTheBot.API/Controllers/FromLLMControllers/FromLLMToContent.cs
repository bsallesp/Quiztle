using BrunoTheBot.API.Controllers.FromLLMControllers;
using BrunoTheBot.API.Controllers.StaticsStatusCodes;
using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.FromLLMToDBControllers
{
    public class FromLLMToContent(IChatGPTRequest chatGPTAPI, FromLLMToLogController fromLLMToLogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly FromLLMToLogController _fromLLMToLogController = fromLLMToLogController;

        public async Task<ActionResult<ContentAPIResponse>> GetNewContentFromLLM(string school, string topicClass, string subTopic)
        {
            try
            {
                var prompt = LLMPrompts.GetNewContentFromSubTopics(school, topicClass, subTopic);
                var responseLLM = await _chatGPTRequest.ChatWithGPT(prompt) ?? throw new Exception();
                await _fromLLMToLogController.SaveLog(nameof(GetNewContentFromLLM), responseLLM);
                var newContent = JSONConverter.ConvertToContent(responseLLM, "NewContent");
                if (string.IsNullOrEmpty(newContent)) throw new Exception("The FromLLMToContent amount is zero or null");

                ContentAPIResponse contentAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    NewContent = newContent
                };

                return contentAPIResponse ?? throw new Exception("FromLLMToContent is null");
            }
            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
        }
    }
}
