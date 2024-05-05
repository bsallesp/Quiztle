using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.CourseControllers.QuestionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateQuestionsController(BookRepository bookRepository, GetQuestionsFromLLM fromLLMToQuestions) : ControllerBase
    {
        private readonly BookRepository _bookRepository = bookRepository;
        private readonly GetQuestionsFromLLM _fromLLMToQuestions = fromLLMToQuestions;

        [HttpPost("CreateQuestionsController")]
        public async Task<ActionResult<BookAPIResponse>> ExecuteAsync([FromBody] Book book)
        {
            try
            {
                var bookAPIResponse = await _fromLLMToQuestions.GetFullNewQuestionsGroupFromLLM(book, 1);
                if (bookAPIResponse.Value!.Status != CustomStatusCodes.SuccessStatus) throw new Exception(bookAPIResponse.Value!.Status);

                await _bookRepository.UpdateBookAsync(book);
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

            return new BookAPIResponse();
        }
    }
}
