using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;

namespace BrunoTheBot.API.Controllers.HeadControllers.Create
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBookController(BookRepository bookDb,
        GetChaptersFromLLM chaptersFromLLM,
        GetAllBookSectionsFromLLM getSectionsFromLLM,
        GetContentFromLLLM fromLLMToContent,
        GetQuestionsFromLLM fromLLMToQuestions
        ) : ControllerBase
    {
        private readonly BookRepository _bookDb = bookDb;
        private readonly GetChaptersFromLLM _getChaptersFromLLM = chaptersFromLLM;
        private readonly GetAllBookSectionsFromLLM _getAllBookSectionsFromLLM = getSectionsFromLLM;
        private readonly GetContentFromLLLM _getContentFromLLM = fromLLMToContent;
        private readonly GetQuestionsFromLLM _getQuestionsFromLLM = fromLLMToQuestions;

        [HttpPost("CreateBookController")]
        public async Task<ActionResult<BookAPIResponse>> ExecuteAsync([FromBody] string bookName, int chaptersAmount = 5, int sectionsAmount = 5)
        {
            try
            {
                var chaptersAPIResponse = await _getChaptersFromLLM.ExecuteAsync(bookName, chaptersAmount);

                if (chaptersAPIResponse.Value?.Status != CustomStatusCodes.SuccessStatus) throw new Exception(chaptersAPIResponse.Value?.Status);
                var newChapters = chaptersAPIResponse.Value?.ChaptersAquired;

                Book newBook = new()
                {
                    Name = bookName,
                    Chapters = newChapters!
                };

                var sectionsAPIResponse = await _getAllBookSectionsFromLLM.ExecuteAsync(newBook, sectionsAmount);
                if (sectionsAPIResponse.Value?.Status != CustomStatusCodes.SuccessStatus) throw new Exception(sectionsAPIResponse.Value?.Status);
                newBook = sectionsAPIResponse.Value!.Book;

                var contentAPIResponse = await _getContentFromLLM.ExecuteAsync(newBook);
                if (contentAPIResponse.Value?.Status != CustomStatusCodes.SuccessStatus) throw new Exception(contentAPIResponse.Value?.Status);
                newBook = contentAPIResponse.Value.Book;

                var questionsContentAPIResponse = await _getQuestionsFromLLM.GetFullNewQuestionsGroupFromLLM(newBook, 1);
                if (questionsContentAPIResponse.Value!.Status == CustomStatusCodes.ErrorStatus) throw new Exception();
                newBook = questionsContentAPIResponse.Value.Book;

                await _bookDb.CreateBookAsync(newBook);

                return sectionsAPIResponse;
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