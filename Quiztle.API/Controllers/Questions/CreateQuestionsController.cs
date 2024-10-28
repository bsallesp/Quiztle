//using Microsoft.AspNetCore.Mvc;
//using Quiztle.API.BackgroundTasks.Questions;
//using Quiztle.DataContext.Repositories.Quiz;

//namespace Quiztle.API.Controllers.Questions
//{
//    {
//    [ApiController]
//    [Route("api/[controller]")]
//    public class CreateQuestionsController : ControllerBase
//    {
//        private readonly QuestionRepository _questionRepository;
//        private readonly BuildQuestionsInBackgroundByLLM _buildQuestionsInBackgroundByLLM;

//        public CreateQuestionsController(
//            QuestionRepository questionRepository,
//            BuildQuestionsInBackgroundByLLM buildQuestionsInBackgroundByLLM
//            )
//        {
//            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
//            _buildQuestionsInBackgroundByLLM = buildQuestionsInBackgroundByLLM;
//        }

//        [HttpPost("draftGuid")]
//        public async Task<ActionResult> ExecuteAsync([FromBody] Guid draftGuid, int questionsAmount)
//        {
//            var result = await _buildQuestionsInBackgroundByLLM.ExecuteAsync(draftGuid);

//            return Ok(result);
//        }
//    }
//}
