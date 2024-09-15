using Quiztle.DataContext.Repositories.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.Tests
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemoveQuestionController : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;

        public RemoveQuestionController(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> ExecuteAsync([FromBody] Guid id)
        {
            var response = await _questionRepository.DeleteQuestionAsync(id);
            if (response == false)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}