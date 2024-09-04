using Quiztle.API.Controllers.LLMControllers;
using Quiztle.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Entities.Course;
using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;

namespace Quiztle.API.Controllers.CourseControllers.BookControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBookByLLMController : ControllerBase
    {
        private readonly BookRepository _bookDb;
        private readonly GetChaptersFromLLM _getChaptersFromLLM;
        private readonly GetAllBookSectionsFromLLM _getAllBookSectionsFromLLM;
        private readonly GetContentFromLLLM _getContentFromLLM;
        private readonly GetQuestionsFromLLM _getQuestionsFromLLM;

        public CreateBookByLLMController(BookRepository bookDb,
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

        [HttpGet("CreateBookByLLMController/{bookName}/{chaptersAmount}/{sectionsAmount}")]
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
                // Captura e registra informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção no CreateBookByLLMController: {ex.Message}";

                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Retorna uma resposta de erro contendo a mensagem detalhada
                return StatusCode(500, new APIResponse<Book>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Message = errorMessage,
                    Data = null
                });
            }

        }
    }
}
