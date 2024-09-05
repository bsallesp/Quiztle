using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.API.Prompts;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.DataContext.Repositories;
using Quiztle.DataContext.Repositories.Quiz;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                Console.WriteLine("Ação já em andamento, não será acionada uma nova solicitação.");
                return new ObjectResult("Ação já em andamento.") { StatusCode = 429 };
            }

            try
            {
                Guid guid = new Guid("5f8440e0-a38b-492a-be47-5a244f7ae16e");
                var scratch = await _scratchRepository.GetScratchByIdAsync(guid);

                int totalPages = scratch!.Drafts!.Count;
                int randomPageIndex = new Random().Next(0, totalPages);
                var llmInput = QuestionsPrompts.GetNewQuestionFromPages(scratch.Drafts[randomPageIndex], 3);

                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    Name = "BuildQuestionsInBackgroundByLLM",
                    JSON = llmInput,
                    Created = DateTime.UtcNow
                });

                var llmResult = await _llmRequest.ExecuteAsync(llmInput, cancellationToken);

                await _aILogRepository.CreateAILogAsync(new CoreBusiness.Log.AILog
                {
                    Name = "BuildQuestionsInBackgroundByLLM",
                    JSON = llmResult,
                    Created = DateTime.UtcNow
                });

                JObject jsonObject = JObject.Parse(llmResult);
                var questionsToken = jsonObject["Questions"];
                if (questionsToken == null) throw new ArgumentException("No 'Questions' found in JSON.");
                var questions = questionsToken.ToObject<List<Question>>();

                var temporaryTest = new Test
                {
                    Id = new Guid("4f5f9086-c5f1-4a9a-8a6f-3f2d1c8f9e65"),
                    Name = "AZ-900: Test inspired in Official Microsoft Content",
                };

                if (await _testRepository.GetTestByIdAsync(temporaryTest.Id) == null)
                    await _testRepository.CreateTestAsync(temporaryTest);

                await _testRepository.AddQuestionsToTestAsync(temporaryTest.Id, questions!);

                return new JsonResult(llmInput) { StatusCode = 200 };
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("BuildQuestionsInBackgroundByLLM/ExecuteAsync was canceled.");
                return new ObjectResult("Request was canceled.") { StatusCode = 499 }; // 499 indicates client closed request
            }
            catch (Exception ex)
            {
                var error = $"Error in BuildQuestionsInBackgroundByLLM/ExecuteAsync: {ex.Message}";
                Console.WriteLine(error);
                return new ObjectResult(error) { StatusCode = 500 };
            }
            finally
            {
                // Libera o semáforo após a execução
                _semaphore.Release();
            }
        }

    }
}
