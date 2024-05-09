using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.CourseControllers.BookControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllBooksController(BookRepository bookDb) : ControllerBase
    {
        private readonly BookRepository _bookDb = bookDb;

        [HttpGet("GetAllBooksController")]
        public async Task<APIResponse<List<Book>>> ExecuteAsync()
        {
            _ = new APIResponse<List<Book>>
            {
                Data = new List<Book>(),
            };

            try
            {
                var result = await _bookDb.GetAllBooksAsync();

                if (result == null) return new APIResponse<List<Book>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Book>(),
                    Message = "Error at getting books: "
                };

                APIResponse<List<Book>> booksAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = result
                };
                return booksAPIResponse;
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