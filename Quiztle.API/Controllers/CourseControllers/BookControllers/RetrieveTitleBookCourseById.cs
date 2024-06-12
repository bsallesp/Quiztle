using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Course;
using Quiztle.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.CourseControllers.BookControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetrieveTitleBookCourseById(BookRepository bookDb) : ControllerBase
    {
        private readonly BookRepository _bookDb = bookDb;


        [HttpPost("RetrieveOnlyBookCourseById")]
        public async Task<ActionResult<APIResponse<Book>>> ExecuteAsync([FromBody] Guid bookId)
        {
            _ = new APIResponse<Book> {Data = new Book()};

            try
            {
                APIResponse<Book> bookAPIResponse = new APIResponse<Book>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = await _bookDb.GetBookByIdAsync(bookId) ?? new Book()
                };

                return bookAPIResponse;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }
                errorMessage += $" StackTrace: {ex.StackTrace}";
                throw new Exception(errorMessage);
            }
        }
    }
}