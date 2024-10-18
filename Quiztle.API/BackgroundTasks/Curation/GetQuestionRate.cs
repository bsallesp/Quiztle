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
            if (!await _curationBackground.CanExecuteAsync())
            {
                Console.WriteLine("Semaphore is red. Exiting execution.");
                return;
            }

            var questions = await _questionRepository.GetRandomQuestionsToRateAsync(false, 5);
            if (questions == null)
            {
                Console.WriteLine("No questions found to rate.");
                return;
            }

            var stringOfQuestions = "";

            foreach (var question in questions)
            {
                stringOfQuestions += question.ToFormattedString();

                Console.WriteLine(question.ToFormattedString());
            }
            
            var resultJson = await _curationBackground.ExecuteAsync(questions);
            Console.WriteLine("Raw JSON result: " + resultJson);

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<AnswerValidation>(resultJson, options);

                if (result != null)
                {
                    Console.WriteLine(result);
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

    public class AnswerValidation
    {
        public Dictionary<int, bool>? Answers { get; set; }
    }

}
