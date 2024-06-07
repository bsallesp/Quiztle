using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Utils;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.CourseControllers.BookControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetrieveBookByIdController : ControllerBase
    {
        private readonly BookRepository _bookDb;

        public RetrieveBookByIdController(BookRepository bookDb)
        {
            _bookDb = bookDb ?? throw new ArgumentNullException(nameof(bookDb));
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<APIResponse<Book>>> ExecuteAsync(Guid bookId)
        {
            try
            {
                var book = await _bookDb.GetBookByIdAsync(bookId, true, true, true);

                if (book == new Book() || book == null)
                {
                    var bookNotFoundAPIResponse = new APIResponse<Book>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new()
                    };

                    return bookNotFoundAPIResponse;
                }

                var bookAPIResponse = new APIResponse<Book>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = book
                };

                return bookAPIResponse;
            }
            catch (Exception ex)
            {
                // Registre a exceção e retorne uma resposta de erro adequada ao invés de lançá-la novamente
                Console.WriteLine($"Erro ao recuperar o livro por ID: {ex.Message}");
                return StatusCode(500, new APIResponse<Book>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Book()
                });
            }
        }
    }
}
