using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Quiztle.DataContext.Repositories.Quiz;
using Quiztle.API.BackgroundTasks.Curation;

namespace Quiztle.API.BackgroundTasks.Curation
{
    public class GetQuestionConfidency
    {
        private readonly QuestionRepository _questionRepository;
        private readonly CurationBackground _curationBackground;

        public GetQuestionConfidency(QuestionRepository questionRepository, CurationBackground curationBackground)
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
            if (questions == null || !questions.Any())
            {
                Console.WriteLine("No questions found to rate.");
                return;
            }

            var stringOfQuestions = string.Join(", ", questions.Select(q => q.ToFormattedString()));

            var resultJson = await _curationBackground.ExecuteAsync(questions);
            Console.WriteLine("Raw JSON result: " + resultJson);

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                // Modificado para usar um dicionário com valores que são também dicionários
                var result = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(resultJson, options);

                if (result != null)
                {
                    Console.WriteLine("Deserialization successful.");

                    foreach (var questionResult in result)
                    {
                        string questionId = questionResult.Key;
                        var answers = questionResult.Value;

                        Console.WriteLine($"Question ID: {questionId}");
                        foreach (var answer in answers)
                        {
                            Console.WriteLine($"Key: {answer.Key}, Value: {answer.Value}");
                        }

                        // Aqui você pode pegar o validatedQuestions com base na questionId
                        var validatedQuestions = await _questionRepository.GetQuestionsByIdsAsync(new[] { Guid.Parse(questionId) });

                        foreach (var question in validatedQuestions)
                        {
                            question.Verified = true;

                            // Atualiza ConfidenceLevel
                            question.ConfidenceLevel += answers.Any(a => a.Key == question.ToFormattedString() && a.Value) ? 1 : -1;

                            Console.WriteLine($"Question {question.Id} verified: {question.Verified}");
                        }
                    }

                    await _questionRepository.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine("Deserialization resulted in null.");
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
        public Dictionary<string, bool> Answers { get; set; } = new Dictionary<string, bool>();
    }
}


/*
SELECT
    SUM(CASE WHEN "ConfidenceLevel" = 1 THEN 1 ELSE 0 END) AS "CountConfidenceLevel1",
    SUM(CASE WHEN "ConfidenceLevel" = -1 THEN 1 ELSE 0 END) AS "CountConfidenceLevelMinus1"
FROM "Questions";
*/