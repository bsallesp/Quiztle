using Quiztle.DataContext.DataService.Repository.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Quiztle.API.Controllers.Tasks
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBookTaskController : ControllerBase
    {
        private readonly BookTaskRepository _bookTaskRepository;

        public CreateBookTaskController(BookTaskRepository bookDb)
        {
            _bookTaskRepository = bookDb;
        }

        [HttpGet("CreateBookTaskController/{bookName}/")]
        public async Task<ActionResult> ExecuteAsync(string bookName)
        {
            try
            {
                bookName = HttpUtility.UrlDecode(bookName);

                Console.WriteLine("Creating new BookTask now!!!");
                var newBookTask = GetNewTempTask(bookName);
                var result = await _bookTaskRepository.CreateBookTaskAsync(newBookTask);

                return Ok("New task in queue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the Book:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        private BookTask GetNewTempTask(string bookName)
        {
            BookTask newBookTask = new BookTask
            {
                User = new CoreBusiness.User
                {
                    Name = "Admin"
                },
                Name = bookName
            };

            return newBookTask;
        }
    }
}
