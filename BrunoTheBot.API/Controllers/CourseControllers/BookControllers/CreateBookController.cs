using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;

namespace BrunoTheBot.API.Controllers.CourseControllers.BookControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBookController : ControllerBase
    {
        private readonly BookRepository _bookDb;
        private readonly GetChaptersFromLLM _getChaptersFromLLM;
        private readonly GetAllBookSectionsFromLLM _getAllBookSectionsFromLLM;
        private readonly GetContentFromLLLM _getContentFromLLM;
        private readonly GetQuestionsFromLLM _getQuestionsFromLLM;

        public CreateBookController(BookRepository bookDb,
            GetChaptersFromLLM chaptersFromLLM,
            GetAllBookSectionsFromLLM getSectionsFromLLM,
            GetContentFromLLLM fromLLMToContent,
            GetQuestionsFromLLM fromLLMToQuestions)
        {
            _bookDb = bookDb;
            _getChaptersFromLLM = chaptersFromLLM;
            _getAllBookSectionsFromLLM = getSectionsFromLLM;
            _getContentFromLLM = fromLLMToContent;
            _getQuestionsFromLLM = fromLLMToQuestions;
        }

        [HttpGet("CreateBookController/{bookName}/{chaptersAmount}/{sectionsAmount}")]
        public async Task<ActionResult<APIResponse<Book>>> ExecuteAsync(string bookName = "Neural NetWork", int chaptersAmount = 5, int sectionsAmount = 5)
        {
            try
            {
                var chaptersAPIResponse = await _getChaptersFromLLM.ExecuteAsync(bookName, chaptersAmount);

                if (chaptersAPIResponse.Value?.Status != CustomStatusCodes.SuccessStatus) throw new Exception(chaptersAPIResponse.Value?.Status);
                var newChapters = chaptersAPIResponse.Value?.Data;

                Book newBook = new()
                {
                    Name = bookName,
                    Chapters = newChapters!
                };

                var sectionsAPIResponse = await _getAllBookSectionsFromLLM.ExecuteAsync(newBook, sectionsAmount);
                if (sectionsAPIResponse.Value?.Status != CustomStatusCodes.SuccessStatus) throw new Exception(sectionsAPIResponse.Value?.Status);
                newBook = sectionsAPIResponse.Value!.Data;

                var contentAPIResponse = await _getContentFromLLM.ExecuteAsync(newBook);
                if (contentAPIResponse.Value?.Status != CustomStatusCodes.SuccessStatus) throw new Exception(contentAPIResponse.Value?.Status);
                newBook = contentAPIResponse.Value.Data;

                var questionsContentAPIResponse = await _getQuestionsFromLLM.ExecuteAsync(newBook, 1);
                if (questionsContentAPIResponse.Value!.Status == CustomStatusCodes.ErrorStatus) throw new Exception();
                newBook = questionsContentAPIResponse.Value.Data;

                await _bookDb.CreateBookAsync(newBook);

                return sectionsAPIResponse;
            }
            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção no CreateBookController: {ex.Message}";

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
