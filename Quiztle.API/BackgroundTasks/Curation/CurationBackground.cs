using System;
using System.Threading;
using System.Threading.Tasks;
using Quiztle.API.Controllers.LLM.Interfaces;

namespace Quiztle.API.BackgroundTasks.Curation
{
    public class CurationBackground
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ILLMRequest _llmRequest;
        private readonly CancellationToken _cancellationToken;

        public CurationBackground(ILLMRequest llmRequest, CancellationToken cancellationToken)
        {
            _llmRequest = llmRequest;
            _cancellationToken = cancellationToken;
        }

        public async Task<string> ExecuteAsync(string quizContent)
        {
            await _semaphore.WaitAsync(_cancellationToken);
            try
            {
                string prompt = GeneratePrompt(quizContent);

                // Check if the operation has been canceled
                if (_cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Operation canceled.");
                    return string.Empty;
                }

                string response = await _llmRequest.ExecuteAsync(prompt, _cancellationToken);
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

        private string GeneratePrompt(string quizContent)
        {
            return $@"
                    I will provide you with a question and answers intended to help study for the AZ-900 Microsoft Azure Fundamentals exam.
                    Your task is to evaluate the overall quality of this quiz based on the following criteria:

                    - Relevance to the AZ-900 exam content
                    - Clarity and accuracy of the questions and answers
                    - Level of difficulty appropriate for someone studying for the AZ-900 exam

                    Please provide a score from 0 to 5 (no decimals allowed) based on the quality of the quiz, and include a brief explanation for the score.
                    The score should reflect the overall usefulness of this quiz for AZ-900 exam preparation.

                    Here is the quiz for evaluation:

                    {quizContent}

                    Return the evaluation in the following JSON format:

                    {{
                      ""score"": [Your score from 0 to 5],
                      ""feedback"": ""[A brief explanation of why you gave this score]"" 
                    }}
                    ";
        }
    }
}
