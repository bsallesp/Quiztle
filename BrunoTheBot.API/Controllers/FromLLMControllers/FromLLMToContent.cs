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

        public async Task<ActionResult<SchoolAPIResponse>> GetFullContentFromLLM(School school)
        {
            try
            {
                foreach (var topic in school.Topics)
                {
                    foreach (var section in topic.Sections)
                    {
                        var prompt = LLMPrompts.GetNewContentFromSection(school.Name, topic.Name, section.Name);
                        var responseLLM = await _chatGPTRequest.ChatWithGPT(prompt) ?? throw new Exception();
                        var registerName = school.Name + topic.Name + section.Name;
                        await _fromLLMToLogController.SaveLog(registerName, responseLLM);
                        var newContent = JSONConverter.ConvertToContent(responseLLM, "NewContent");
                        if (string.IsNullOrEmpty(newContent)) throw new Exception("The FromLLMToContent amount is zero or null");

                        section.Content.Text = newContent;
                        Console.WriteLine(newContent);
                    }
                }

                SchoolAPIResponse schoolAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    School = school
                };

                return schoolAPIResponse ?? throw new Exception("schoolAPIResponse show some error: ");
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
