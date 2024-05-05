using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.CourseControllers.BookControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetrieveTitleBookCourseById(BookRepository bookDb) : ControllerBase
    {
        private readonly BookRepository _bookDb = bookDb;


        [HttpPost("RetrieveOnlyBookCourseById")]
        public async Task<ActionResult<BookAPIResponse>> ExecuteAsync([FromBody] int bookId)
        {
            _ = new BookAPIResponse();

            try
            {
                BookAPIResponse bookAPIResponse = new BookAPIResponse
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Book = await _bookDb.GetBookByIdAsync(bookId) ?? new Book()
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