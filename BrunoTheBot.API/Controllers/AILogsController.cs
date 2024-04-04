using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BrunoTheBot.DataContext;
using BrunoTheBot.CoreBusiness.Log;

namespace BrunoTheBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AILogsController : ControllerBase
    {
        private readonly PostgreBrunoTheBotContext _context;

        public AILogsController(PostgreBrunoTheBotContext context)
        {
            _context = context;
        }

        // GET: api/AILogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AILog>>> Index()
        {
            var logs = _context.AILogs?.ToListAsync();
            if (logs == null)
            {
                return NotFound();
            }
            return await logs;
        }

        //// GET: api/AILogs/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<AILog>> Details(int id)
        //{
        //    var aILog = await _context.AILogs.FindAsync(id);

        //    if (aILog == null)
        //    {
        //        return NotFound();
        //    }

        //    return aILog;
        //}

        //// POST: api/AILogs
        //[HttpPost]
        //public async Task<ActionResult<AILog>> Create([FromBody]AILog aILog)
        //{
        //    Console.WriteLine("JSON AT HTTPPOST API:: " + aILog.JSON);

        //    if (aILog == null)
        //    {
        //        return BadRequest();
        //    }

        //    _context.AILogs.Add(aILog);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(Details), new { id = aILog.Id }, aILog);
        //}

        //// PUT: api/AILogs/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Edit(int id, AILog aILog)
        //{
        //    if (aILog == null || id != aILog.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var existingLog = await _context.AILogs.FindAsync(id);
        //    if (existingLog == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Entry(existingLog).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AILogExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// DELETE: api/AILogs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var aILog = await _context.AILogs.FindAsync(id);
        //    if (aILog == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.AILogs.Remove(aILog);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool AILogExists(int id)
        //{
        //    return _context.AILogs.Any(e => e.Id == id);
        //}
    }
}
