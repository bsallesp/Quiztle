using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.DataContext.Repositories.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.CourseControllers.QuestionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateQuestionsFromStringTokens(QuestionRepository questionRepository, GetQuestionsFromLLMAndTokens getQuestionsFromLLMAndTokens) : ControllerBase
    {
        private readonly QuestionRepository _questionRepository = questionRepository;
        private readonly GetQuestionsFromLLMAndTokens _getQuestionsFromLLMAndTokens = getQuestionsFromLLMAndTokens;

        [HttpPost("CreateQuestionsFromStringTokens")]
        public async Task<ActionResult<APIResponse<List<Question>>>> ExecuteAsync(string tokensStringPart)
        {

            try
            {
                var questions = await _getQuestionsFromLLMAndTokens.ExecuteAsync(tokensStringPart, 10);
                if (questions.Value!.Status != CustomStatusCodes.SuccessStatus) throw new Exception(questions.Value!.Status);

                //await _questionRepository.UpdateBookAsync(book);
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

            return new APIResponse<List<Question>> { Data  = new List<Question>() };
        }
    }
}
