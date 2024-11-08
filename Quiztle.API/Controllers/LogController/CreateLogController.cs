using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.Repositories;
using Quiztle.CoreBusiness.Log;

namespace Quiztle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateLogController : ControllerBase
    {
        private readonly LogRepository _logRepository;

        public CreateLogController(LogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] Log log)
        {
            await _logRepository.CreateLogAsync(log);

            return Ok(log);
        }
    }
}