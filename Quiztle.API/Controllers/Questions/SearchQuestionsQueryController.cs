using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.DataContext.Repositories.Quiz
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchQuestionsQueryController : ControllerBase
    {
        private readonly PostgreQuiztleContext _context;

        public SearchQuestionsQueryController(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Busca questões que correspondem às palavras-chave fornecidas.
        /// </summary>
        /// <param name="keywords">Array de strings com palavras-chave.</param>
        /// <returns>Lista de questões que correspondem às palavras-chave.</returns>
        /// 
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Question>>> SearchQuestionsByKeywordsAsync([FromBody] string[] keywords)
        {
            if (keywords == null || keywords.Length == 0)
                return Ok(Enumerable.Empty<Question>());

            var keywordLowered = keywords.Select(k => k.ToLower()).ToList();

            var questions = await _context.Questions!
                .Where(q => keywordLowered.Any(k =>
                    q.Name.ToLower().Contains(k) ||
                    (q.Hint != null && q.Hint.ToLower().Contains(k)) ||
                    (q.Resolution != null && q.Resolution.ToLower().Contains(k)) ||
                    q.Options.Any(o => o.Name.ToLower().Contains(k))))
                .Include(o => o.Options)
                .ToListAsync();

            return Ok(questions);
        }
    }
}
