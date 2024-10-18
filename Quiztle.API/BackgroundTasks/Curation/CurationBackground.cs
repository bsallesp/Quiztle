using Quiztle.API.BackgroundTasks.Questions;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.API.Prompts;
using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.API.BackgroundTasks.Curation
{
    public class CurationBackground
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ILLMChatGPTRequest _llmRequest;
        private readonly CancellationToken _cancellationToken;
        private readonly AnswerValidateQuestions _answerValidateQuestions;

        public CurationBackground(ILLMChatGPTRequest llmRequest,
            CancellationToken cancellationToken,
            AnswerValidateQuestions answerValidateQuestions)
        {
            _llmRequest = llmRequest;
            _cancellationToken = cancellationToken;
            _answerValidateQuestions = answerValidateQuestions;
        }

        public async Task<string> ExecuteAsync(IEnumerable<Question> questions)
        {
            await _semaphore.WaitAsync(_cancellationToken);
            try
            {
                string prompt = Prompts.CurationPrompt.GenerateAnswerValidationPrompt(questions);

                if (_cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Operation canceled.");
                    return string.Empty;
                }

                string response = await _llmRequest.ExecuteAsync(prompt);

                await _answerValidateQuestions.ExecuteAsync();
                
                //await _removeBadQuestions.ExecuteAsync();

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
