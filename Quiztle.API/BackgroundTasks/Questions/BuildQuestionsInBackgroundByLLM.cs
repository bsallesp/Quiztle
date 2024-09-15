using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.API.Prompts;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.DataContext.Repositories;
using Quiztle.DataContext.Repositories.Quiz;

namespace Quiztle.API.BackgroundTasks.Questions
{
    public class BuildQuestionsInBackgroundByLLM
    {
        private readonly ILLMRequest _llmRequest;
        private readonly ScratchRepository _scratchRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly AILogRepository _aILogRepository;
        private readonly TestRepository _testRepository;
        private static CancellationTokenSource? _cts;
        private static DateTime _lastRequestTime = DateTime.MinValue;
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromMinutes(5);

        // Cria um semáforo que limita o número de execuções simultâneas
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public BuildQuestionsInBackgroundByLLM(
            ILLMRequest llmRequest,
            ScratchRepository scratchRepository,
            QuestionRepository questionRepository,
            AILogRepository aILogRepository,
            TestRepository testRepository
        )
        {
            _llmRequest = llmRequest;
            _scratchRepository = scratchRepository;
            _questionRepository = questionRepository;
            _aILogRepository = aILogRepository;
            _testRepository = testRepository;
        }

        public async Task<IActionResult> ExecuteAsync()
        {
            var now = DateTime.UtcNow;

            // Cancelamento e limpeza do token de cancelamento
            if (_cts != null && now - _lastRequestTime > RequestTimeout)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            _cts = new CancellationTokenSource();
            var cancellationToken = _cts.Token;
            _lastRequestTime = DateTime.UtcNow;

            // Verifica se o semáforo está disponível
            if (!await _semaphore.WaitAsync(TimeSpan.FromSeconds(1)))
            {
                return new ObjectResult("Ação já em andamento.") { StatusCode = 429 };
            }

            try
            {
                var scratches = await _scratchRepository.GetAllScratchesAsync();

                // Seleciona um scratch aleatório
                var randomScratch = scratches.OrderBy(s => Guid.NewGuid()).FirstOrDefault();
                if (randomScratch == null) throw new InvalidOperationException("No scratches found.");

                // Seleciona um draft aleatório do scratch selecionado
                var randomDraft = randomScratch.Drafts!.OrderBy(d => Guid.NewGuid()).FirstOrDefault();
                if (randomDraft == null) throw new InvalidOperationException("No drafts found in the selected scratch.");

                var questionsList = await _questionRepository.GetQuestionByDraftAsync(randomDraft.Id);
                var descriptionOfQuestions = questionsList.Select(d => d!.Name.ToString()).ToList();

                var llmInput = QuestionsPrompts.GetNewQuestionFromPages(randomDraft.Text, descriptionOfQuestions, 3);

                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    JSON = llmInput,
                    Name = "BuildQuestionsInBackgroundByLLM - " + "llmInput"
                });

                var llmResult = await _llmRequest.ExecuteAsync(llmInput, cancellationToken);

                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    JSON = llmResult,
                    Name = "BuildQuestionsInBackgroundByLLM - " + "llmResult"
                });

                JObject jsonObject = JObject.Parse(llmResult);
                var questionsToken = jsonObject["Questions"];
                if (questionsToken == null) throw new ArgumentException("No 'Questions' found in JSON.");
                var questions = questionsToken.ToObject<List<Question>>();

                foreach (var question in questions!) question.Draft = randomDraft;

                var temporaryTest = new Test
                {
                    Id = new Guid("4f5f9086-c5f1-4a9a-8a6f-3f2d1c8f9e65"),
                    Name = "AZ-900",
                };

                if (await _testRepository.GetTestByIdAsync(temporaryTest.Id) == null)
                    await _testRepository.CreateTestAsync(temporaryTest);

                await _testRepository.AddQuestionsToTestAsync(temporaryTest.Id, questions!);
                Console.WriteLine("Questions make by LLM Finished.");

                return new JsonResult(new { message = "All drafts processed" }) { StatusCode = 200 };

            }
            catch (OperationCanceledException)
            {
                return new ObjectResult("Request was canceled.") { StatusCode = 499 };
            }
            catch (Exception ex)
            {
                var error = $"Error in BuildQuestionsInBackgroundByLLM/ExecuteAsync: {ex.Message}";
                return new ObjectResult(error) { StatusCode = 500 };
            }
            finally
            {
                _semaphore.Release();
            }
        }

    }
}
