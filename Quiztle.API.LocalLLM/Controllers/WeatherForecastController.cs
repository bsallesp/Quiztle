using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Quiztle.API.LocalLLM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuntimeController : ControllerBase
    {
        private static Runtime _runtime = new Runtime(); // Instância estática para simplificação

        [HttpGet(Name = "Start")]
        public async Task<IActionResult> Start()
        {
            await _runtime.Start();
            return Ok("Runtime started.");
        }

        [HttpGet("stop")]
        public IActionResult Stop()
        {
            _runtime.Stop();
            return Ok("Runtime stopped.");
        }

        [HttpGet("updateInterval/{interval}")]
        public IActionResult UpdateInterval(int interval)
        {
            if (interval <= 0)
            {
                return BadRequest("Interval must be a positive number.");
            }

            _runtime.UpdateInterval(interval);
            return Ok($"Interval updated to {interval} milliseconds.");
        }
    }
}
