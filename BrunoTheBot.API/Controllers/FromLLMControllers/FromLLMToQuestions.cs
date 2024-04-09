using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.FromLLMControllers
{
    public class FromLLMToQuestions(IChatGPTRequest chatGPTAPI, FromLLMToLogController fromLLMToLogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly FromLLMToLogController _fromLLMToLogController = fromLLMToLogController;

        public async Task<ActionResult<SchoolAPIResponse>> GetFullNewQuestionsGroupFromLLM(School school, int questionsPerSection = 1)
        {
            try
            {
                if (school == null || school.Topics.Count <= 0) return new SchoolAPIResponse
                {
                    Status = CustomStatusCodes.EmptyObjectErrorStatus,
                    School = new ()
                };

                foreach (var topic in school.Topics) 
                {
                    foreach (var section in topic.Sections)
                    {
                        var prompt = LLMPrompts.GetNewQuestion(section.Content.Text!, questionsPerSection);
                        var responseLLM = await _chatGPTRequest.ChatWithGPT(prompt) ?? throw new Exception();
                        await _fromLLMToLogController.SaveLog(nameof(GetFullNewQuestionsGroupFromLLM), responseLLM);
                        var newQuestion = JSONConverter.ConvertToQuestion(responseLLM);

                        Console.WriteLine(newQuestion.Name);
                        Console.WriteLine(newQuestion.Answer);

                        section.Questions.Add(newQuestion);
                    }
                }

                SchoolAPIResponse schoolAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    School = school
                };

                return schoolAPIResponse;

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
