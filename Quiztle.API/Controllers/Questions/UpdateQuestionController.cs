using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.Repositories.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace Quiztle.API.Controllers.Questions
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateQuestionController : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;

        public UpdateQuestionController(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse<bool>>> UpdateQuestionAsync(Guid id, [FromBody] Question updatedQuestion)
        {
            if (updatedQuestion == null || id != updatedQuestion.Id)
            {
                return BadRequest();
            }

            var response = await _questionRepository.UpdateQuestionAsync(id, updatedQuestion);
            if (response == false)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
