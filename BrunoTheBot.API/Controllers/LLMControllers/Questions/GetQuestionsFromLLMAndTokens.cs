using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.LLMControllers
{
    public class GetQuestionsFromLLMAndTokens(IChatGPTRequest chatGPTAPI, SaveAILogController fromLLMToLogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly SaveAILogController _fromLLMToLogController = fromLLMToLogController;

        public async Task<ActionResult<APIResponse<List<Question>>>> ExecuteAsync(string tokensStringPart, int questionsPerSection = 1)
        {
            try
            {
                if (tokensStringPart == null || questionsPerSection <= 0) return new APIResponse<List<Question>>
                {
                    Status = CustomStatusCodes.EmptyObjectErrorStatus,
                    Data = []
                };

                var prompt = CreateQuestionsTokensPrompts.GetQuestionsFromPartOfPDFString(tokensStringPart);
                var responseLLM = await _chatGPTRequest.ExecuteAsync(prompt) ?? throw new Exception();
                await _fromLLMToLogController.ExecuteAsync(nameof(ExecuteAsync), responseLLM);

                Console.WriteLine(responseLLM);

                APIResponse<List<Question>> questionsAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = new List<Question>()
                };

                return questionsAPIResponse;

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
