using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.DataContext;
using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly PostgreBrunoTheBotContext _context;

        public SchoolsController(PostgreBrunoTheBotContext context)
        {
            _context = context;
        }

        // GET: api/Schools
        [HttpGet]
        public ActionResult<IEnumerable<School>> Index()
        {
            try
            {
                var schools = _context.Schools ?? throw new Exception();
                var schoolsWithTopics = _context.Schools.Include(s => s.Topics).ToList();

                return Ok(schoolsWithTopics.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving schools: {ex.Message}");
            }
        }
    }
}

