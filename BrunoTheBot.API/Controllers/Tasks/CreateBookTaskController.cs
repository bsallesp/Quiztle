using BrunoTheBot.CoreBusiness.Entities.Tasks;
using BrunoTheBot.DataContext.DataService.Repository.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.Tasks
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
        public async Task<bool> ExecuteAsync(string bookName)
        {
            try
            {
                Console.WriteLine("Creating new BookTask now!!!");
                var newBookTask = GetNewTempTask(bookName);
                var result = await _bookTaskRepository.CreateBookTaskAsync(newBookTask);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the Book:");
                Console.WriteLine(ex.ToString());
                throw;
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
