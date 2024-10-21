using Microsoft.AspNetCore.Mvc;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.API.Prompts;
using Quiztle.CoreBusiness.Entities.Scratch;
using Quiztle.CoreBusiness.Log;
using Quiztle.CoreBusiness.Utils;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.Repositories;
using Quiztle.DataContext.Repositories.Quiz;

namespace Quiztle.API.Controllers.CurationController

{
    [ApiController]
    [Route("api/[controller]")]
    public class AIAnswerQuestionsController : ControllerBase
    {
        private readonly ScratchRepository _scratchRepository;
        private readonly AILogRepository _aILogRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly DraftRepository _DraftRepository;
        private readonly ILLMChatGPTRequest _llm;
        private readonly ILogger<AIAnswerQuestionsController> _logger;

        public AIAnswerQuestionsController(
            ScratchRepository scratchRepository,
            AILogRepository aILogRepository,
            QuestionRepository questionRepository,
            ILLMChatGPTRequest llm,
            DraftRepository draftRepository,
            ILogger<AIAnswerQuestionsController> logger
            )
        {
            _scratchRepository = scratchRepository;
            _aILogRepository = aILogRepository;
            _questionRepository = questionRepository;
            _llm = llm ?? throw new ArgumentNullException();
            _DraftRepository = draftRepository;
            _logger = logger;
        }


        [HttpPost("Draft")]
        public async Task<ActionResult> GetAIAnswerByDraftAsync(Guid draftGuid, int verifiedTimesLevel = 3)
        {
            var myDraft = await _DraftRepository.GetDraftByIdAsync(draftGuid);

            if (myDraft == null || myDraft.Questions == null)
            {
                return NotFound("Draft or questions not found");
            }


            while (myDraft.Questions.Where(q => q.VerifiedTimes < verifiedTimesLevel).Any())
            {
                foreach (var question in myDraft.Questions.Where(q => q.VerifiedTimes < verifiedTimesLevel))
                {
                    var prompt = CurationPrompt.AIPlayQuestionsPrompt(myDraft.OriginalContent, question);

                    var lLMJSONResult = await _llm.ExecuteAsync(prompt);

                    await _aILogRepository.CreateAILogAsync(new AILog
                    {
                        Created = DateTime.UtcNow,
                        Name = "AIAnswerQuestionsController / CurationPrompt",
                        JSON = lLMJSONResult
                    });

                    if (!JsonExtractor.TryFindGuidInJson(lLMJSONResult, out Guid foundGuid))
                        _logger.LogError("Guid NOT FOUND no JSON");

                    var oldCorrectOption = question.Options.FirstOrDefault(o => o.IsCorrect);

                    question.AddAIAnswer(oldCorrectOption!.Id == foundGuid);

                    await _DraftRepository.SaveChangesAsync();
                }
            }

            return Ok();
        }

        [HttpPost("Scratch")]
        public async Task<ActionResult> GetAIAnswerByScratch(Guid scratchGuid, int verifiedTimesLevel = 3)
        {
            var retrievedScratch = await _scratchRepository.GetScratchByIdAsync(scratchGuid);

            if (retrievedScratch == null) return NotFound();

            foreach (var draft in retrievedScratch.Drafts!) await GetAIAnswerByDraftAsync(draft.Id, verifiedTimesLevel);

            return Ok(retrievedScratch);
        }
    }
}

