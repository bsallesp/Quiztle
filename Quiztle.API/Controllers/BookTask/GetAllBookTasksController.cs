using Quiztle.CoreBusiness.Entities.Tasks;
using Quiztle.DataContext.DataService.Repository.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Quiztle.API.Controllers.Tasks
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllBookTasksController : ControllerBase
    {
        private readonly BookTaskRepository _bookTaskRepository;

        public GetAllBookTasksController(BookTaskRepository bookDb)
        {
            _bookTaskRepository = bookDb;
        }

        [HttpGet("GetAllBookTasks")]
        public async Task<List<BookTask>> ExecuteAsync()
        {
            try
            {
                var allBookTasks = await _bookTaskRepository.GetAllBookTasks();
                return allBookTasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while getting all the BookTasks:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
