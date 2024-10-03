using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Course;
using Quiztle.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.CourseControllers.BookControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetrieveBookAndChaptersCourseById(BookRepository bookDb) : ControllerBase
    {
        private readonly BookRepository _bookDb = bookDb;

        [HttpPost("RetrieveBookAndChaptersCourseById")]
        public async Task<ActionResult<APIResponse<Book>>> ExecuteAsync([FromBody] Guid bookId)
        {
            try
            {
                var restult = await _bookDb.GetBookByIdAsync(bookId, true) ?? new Book();

                if (restult == null) return NotFound(restult);
                return Ok(restult);
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