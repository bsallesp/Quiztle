using Microsoft.AspNetCore.Mvc;
using Quiztle.DataContext.Repositories.Quiz;
using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.API.Controllers.Questions
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetQuestionsController : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;

        public GetQuestionsController(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        }

        // GET: api/GetQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetAllQuestionsAsync()
        {
            try
            {
                var questions = await _questionRepository.GetAllQuestionsAsync();
                return Ok(questions);
            }
            catch (Exception ex)
            {
                // Log error here
                return StatusCode(500, "An error occurred while retrieving questions.");
            }
        }

        // GET: api/GetQuestions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestionByIdAsync(Guid id)
        {
            try
            {
                var question = await _questionRepository.GetQuestionByIdAsync(id);
                if (question == null)
                {
                    return NotFound();
                }

                return Ok(question);
            }
            catch (Exception ex)
            {
                // Log error here
                return StatusCode(500, "An error occurred while retrieving the question.");
            }
        }
    }
}
