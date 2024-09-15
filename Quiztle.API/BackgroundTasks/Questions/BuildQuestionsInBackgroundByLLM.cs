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
using System.Collections.Generic;
using System.Linq;
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

        // Semáforo para limitar execuções simultâneas
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
                return new ObjectResult("Ação já em andamento.") { StatusCode = 429 };
            }

            try
            {
                var scratches = await _scratchRepository.GetAllScratchesAsync();

                var randomScratch = scratches.OrderBy(s => Guid.NewGuid()).FirstOrDefault();
                if (randomScratch == null) throw new InvalidOperationException("No scratches found.");

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

                var test = await _testRepository.GetTestByNameAsync(randomScratch.Name!);
                if (test == null)
                {
                    test = new Test
                    {
                        Name = randomScratch.Name!,
                        Questions = questions
                    };

                    await _testRepository.CreateTestAsync(test);
                }

                var existingQuestionIds = test.Questions.Select(q => q.Id).ToHashSet();
                var newQuestions = questions.Where(q => !existingQuestionIds.Contains(q.Id)).ToList();

                if (newQuestions.Any())
                {
                    test.Questions.AddRange(newQuestions);
                    await _testRepository.UpdateTest(test);
                }

                Console.WriteLine("Questions generated by LLM finished.");

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
