using Quiztle.API.BackgroundTasks.Questions;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.API.Prompts;
using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.API.BackgroundTasks.Curation
{
    public class CurationBackground
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ILLMRequest _llmRequest;
        private readonly CancellationToken _cancellationToken;
        private readonly RemoveBadQuestions _removeBadQuestions;

        public CurationBackground(ILLMRequest llmRequest, CancellationToken cancellationToken, RemoveBadQuestions removeBadQuestions)
        {
            _llmRequest = llmRequest;
            _cancellationToken = cancellationToken;
            _removeBadQuestions = removeBadQuestions;
        }

        public async Task<string> ExecuteAsync(Question question)
        {
            await _semaphore.WaitAsync(_cancellationToken);
            try
            {
                string prompt = Prompts.CurationPrompt.GeneratePrompt(question);

                // Check if the operation has been canceled
                if (_cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Operation canceled.");
                    return string.Empty;
                }

                string response = await _llmRequest.ExecuteAsync(prompt, _cancellationToken);
                await _removeBadQuestions.ExecuteAsync();

                return response;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation canceled.");
                return string.Empty;
            }
            finally
            {
                // Release the semaphore
                _semaphore.Release();
            }
        }

        public Task<bool> CanExecuteAsync()
        {
            // If semaphore is not available (i.e., the count is 0), return false
            return Task.FromResult(_semaphore.CurrentCount > 0);
        }
    }
}
