using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.HeadControllers.Retrieve
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