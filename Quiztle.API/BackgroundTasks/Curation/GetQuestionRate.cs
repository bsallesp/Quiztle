using System;
using System.Text.Json;
using System.Threading.Tasks;
using Quiztle.DataContext.Repositories.Quiz;
using Quiztle.API.BackgroundTasks.Curation;

namespace Quiztle.API.BackgroundTasks.Curation
{
    public class GetQuestionRate
    {
        private readonly QuestionRepository _questionRepository;
        private readonly CurationBackground _curationBackground;

        public GetQuestionRate(QuestionRepository questionRepository, CurationBackground curationBackground)
        {
            _questionRepository = questionRepository;
            _curationBackground = curationBackground;
        }

        public async Task ExecuteAsync()
        {
            // Check the semaphore state before proceeding
            if (!await _curationBackground.CanExecuteAsync())
            {
                Console.WriteLine("Semaphore is red. Exiting execution.");
                return;
            }

            var question = await _questionRepository.GetARandomQuestionToRate();
            if (question == null)
            {
                Console.WriteLine("No questions found to rate.");
                return;
            }

            Console.WriteLine(question.ToFormattedString());

            var resultJson = await _curationBackground.ExecuteAsync(question);

            // Log the raw JSON result
            Console.WriteLine("Raw JSON result: " + resultJson);

            try
            {
                // Ensure correct JSON format and deserialization
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<QuizEvaluationResult>(resultJson, options);

                if (result != null)
                {
                    // Update the question's rating based on the evaluation
                    question.Verified = true;
                    question.Consistency = result.Consistency != 0;
                    question.Rate = result.Score;

                    await _questionRepository.UpdateQuestionAsync(question);
                    Console.WriteLine($"Question rated with score: {result.Score}");
                    Console.WriteLine($"Question consistencty is: {result.Consistency}");
                }
                else
                {
                    Console.WriteLine("Failed to parse the evaluation result.");
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
            }
        }
    }

    public class QuizEvaluationResult
    {
        public int Score { get; set; }
        public int Consistency { get; set; }
    }
}
