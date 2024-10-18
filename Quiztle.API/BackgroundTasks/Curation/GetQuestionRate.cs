using System;
using System.Collections.Generic;
using System.Linq;
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

                var result = JsonSerializer.Deserialize<Dictionary<string, string>>(resultJson, options);

                if (result != null)
                {
                    var answers = result.ToDictionary(kv => kv.Key, kv => kv.Value == "t");
                    Console.WriteLine("Deserialization successful. Answers: " + (answers != null ? string.Join(", ", answers.Keys) : "None"));

                    if (answers != null && answers.Any())
                    {
                        Console.WriteLine("Answers:");
                        foreach (var answer in answers)
                        {
                            Console.WriteLine($"Key: {answer.Key}, Value: {answer.Value}");
                        }

                        var validatedQuestions = await _questionRepository.GetQuestionsByIdsAsync(answers.Keys.Select(Guid.Parse).ToArray());

                        foreach (var question in validatedQuestions)
                        {
                            question.Verified = true;

                            foreach (var answer in answers)
                            {
                                if (question.Id.ToString() == answer.Key)
                                {
                                    question.ConfidenceLevel += answer.Value ? 1 : -1;
                                    break;
                                }
                            }

                            Console.WriteLine($"Question {question.Id} verified: {question.Verified}");
                        }

                        await _questionRepository.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine("No answers found in the result.");
                    }
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
