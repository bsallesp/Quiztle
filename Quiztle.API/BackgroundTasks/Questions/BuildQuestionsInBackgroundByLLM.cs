using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.API.Prompts;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.CoreBusiness.Entities.Scratch;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.DataContext.Repositories;
using Quiztle.DataContext.Repositories.Quiz;
using System.Data;

namespace Quiztle.API.BackgroundTasks.Questions
{
    public class BuildQuestionsInBackgroundByLLM
    {
        private readonly ILLMRequest _llmRequest;
        private readonly AILogRepository _aILogRepository;
        private readonly TestRepository _testRepository;
        private readonly DraftRepository _draftRepository;
        private static CancellationTokenSource? _cts;
        private static DateTime _lastRequestTime = DateTime.MinValue;
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromMinutes(5);
        private bool debugCW = false;

        // Semáforo para limitar execuções simultâneas
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public BuildQuestionsInBackgroundByLLM(
            ILLMRequest llmRequest,
            AILogRepository aILogRepository,
            TestRepository testRepository,
            DraftRepository draftRepository
,
            QuestionRepository questionRepository)
        {
            _llmRequest = llmRequest;
            _aILogRepository = aILogRepository;
            _testRepository = testRepository;
            _draftRepository = draftRepository;
        }

        public async Task<IActionResult> ExecuteAsync()
        {
            var now = DateTime.UtcNow;

            if (_cts != null && now - _lastRequestTime > RequestTimeout)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            _cts = new CancellationTokenSource();
            var cancellationToken = _cts.Token;
            _lastRequestTime = DateTime.UtcNow;

            if (!await _semaphore.WaitAsync(TimeSpan.FromSeconds(1)))
            {
                Console.WriteLine("Red light at " + DateTime.UtcNow.ToString() + "...");
                return new ObjectResult("Ação já em andamento.") { StatusCode = 429 };
            }

            try
            {
                if (debugCW) Console.WriteLine("Running BuildQuestionsInBackgroundByLLM/ExecuteAsync...");

                var draft = await _draftRepository.GetNextDraftToMakeQuestionsAsync() ?? throw new Exception("No drafts.");
                if (draft == null) throw new Exception("DRAFT IS NULL");
                if (debugCW) Console.WriteLine("Draft isnt null, going forward...");
                if (debugCW) Console.WriteLine("Total questions in draft: " + draft!.Questions!.Count);

                if (debugCW) Console.WriteLine("Getting resul from LLM...");

                var stringQuestions = draft?.Questions?
                    .Where(q => !string.IsNullOrEmpty(q.Name))
                    .Select(q => q.Name)
                    .ToList();

                foreach(var name in stringQuestions ?? new List<string>()) Console.WriteLine(name);

                var llmInput = Prompts.QuestionsPrompts.GetNewQuestionFromPages(draft!.Text, stringQuestions, 3);
                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    JSON = llmInput,
                    Name = "BuildQuestionsInBackgroundByLLM - " + "llmInput"
                });


                var llmRequestResult = await _llmRequest.ExecuteAsync(llmInput, cancellationToken);
                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    JSON = llmRequestResult,
                    Name = "BuildQuestionsInBackgroundByLLM - " + "llmResult"
                });

                JObject jsonObject = JObject.Parse(llmRequestResult);
                var questionsToken = jsonObject["Questions"] ?? throw new ArgumentException("No 'Questions' found in JSON.");
                var questions = questionsToken.ToObject<List<Question>>();
                if (debugCW) Console.WriteLine("QuestionsToken: " + questionsToken);

                if (questions == null)
                {
                    Console.WriteLine("QUESTIONS ARE NULL. FNINSHING THE SCOPE.");
                    throw new Exception("QUESTIONS ARE NULL. FNINSHING THE SCOPE.");
                }


                if (debugCW) Console.WriteLine(llmRequestResult);

                if (debugCW) Console.WriteLine("QUESTIONS SET ADQUIRED. MOVING QUESTIONS TO DRAFT...");
                if (debugCW) Console.WriteLine("Total questions in draft: " + draft!.Questions!.Count);
                if (draft.Questions == null) draft.Questions = [];
                draft.Questions.AddRange(questions);

                if (draft.Questions == null) Console.WriteLine("questions eh nulo");
                if (draft.Questions == null) throw new Exception("questions eh nulo");

                foreach (var question in draft.Questions) Console.WriteLine(question.Name);

                await _draftRepository.UpdateDraftAsync(draft);

                if (debugCW) Console.WriteLine("Questions generated by LLM finished.");
                return new JsonResult(new { message = "All drafts processed" }) { StatusCode = 200 };
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Request was canceled.");
                return new ObjectResult("Request was canceled.") { StatusCode = 499 };
            }
            catch (Exception ex)
            {
                // Detalhes do erro para diagnóstico
                var errorMessage = $"Error in BuildQuestionsInBackgroundByLLM/ExecuteAsync: {ex.Message}";
                var stackTrace = ex.StackTrace ?? "No stack trace available";

                // Log completo do erro
                var detailedError = new
                {
                    Error = errorMessage,
                    StackTrace = stackTrace,
                    ExceptionType = ex.GetType().Name,
                    InnerException = ex.InnerException?.Message ?? "No inner exception",
                    Timestamp = DateTime.UtcNow,
                    InputData = new { /* adicione aqui variáveis relevantes para o contexto */ }
                };

                // Registrar no sistema de logs
                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    JSON = JsonConvert.SerializeObject(detailedError),
                    Name = "BuildQuestionsInBackgroundByLLM - Error"
                });

                // Log para depuração imediata (se necessário)
                Console.WriteLine(JsonConvert.SerializeObject(detailedError, Formatting.Indented));

                // Retornar mensagem amigável para o usuário (ou API)
                var userFriendlyMessage = "An unexpected error occurred while processing your request. Please try again later.";
                return new ObjectResult(userFriendlyMessage) { StatusCode = 500 };
            }

            finally
            {
                Console.WriteLine("Releasing semaphore...");
                _semaphore.Release();
            }

        }
    }
}
