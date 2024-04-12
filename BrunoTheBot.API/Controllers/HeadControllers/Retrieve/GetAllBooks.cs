using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.HeadControllers.Retrieve
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllBooks(BookRepository bookDb) : ControllerBase
    {
        private readonly BookRepository _bookDb = bookDb;

        [HttpGet("GetAllBooks")]
        public async Task<BooksAPIResponse> ExecuteAsync()
        {
            _ = new BooksAPIResponse();

            try
            {
                BooksAPIResponse booksAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Books = await _bookDb.GetAllBooksAsync()
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