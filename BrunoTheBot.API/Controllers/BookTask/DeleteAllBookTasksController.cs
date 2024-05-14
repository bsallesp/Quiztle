using BrunoTheBot.DataContext.DataService.Repository.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BrunoTheBot.API.Controllers.Tasks
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteAllBookTasksController : ControllerBase
    {
        private readonly BookTaskRepository _bookTaskRepository;

        public DeleteAllBookTasksController(BookTaskRepository bookDb)
        {
            _bookTaskRepository = bookDb;
        }

        [HttpDelete("DeleteAllBookTasks")]
        public async Task<IActionResult> ExecuteAsync()
        {
            try
            {
                bool success = await _bookTaskRepository.DeleteAll();

                if (success)
                {
                    return Ok("All BookTasks deleted successfully.");
                }
                else
                {
                    return BadRequest("Failed to delete all BookTasks.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while deleting all the BookTasks:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error occurred.");
            }
        }
    }
}
