using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Quiztle.CoreBusiness.Log;
using Quiztle.DataContext.Repositories;
using Quiztle.DataContext.Repositories.Quiz;

namespace Quiztle.API.BackgroundTasks.Curation
{
    public class GetQuestionConfidency : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;
        private readonly CurationBackground _curationBackground;
        private readonly AILogRepository _aiLogRepository;
        private readonly ILogger<GetQuestionConfidency> _logger;

        private static readonly Guid QuizId = Guid.Parse("e01f9b04-acac-4766-9827-57f2cdf75e2e"); // Constant for Quiz ID

        public GetQuestionConfidency(
            QuestionRepository questionRepository,
            CurationBackground curationBackground,
            AILogRepository aiLogRepository,
            ILogger<GetQuestionConfidency> logger)
        {
            _questionRepository = questionRepository;
            _curationBackground = curationBackground;
            _aiLogRepository = aiLogRepository; // Fixed typo
            _logger = logger;
        }

        private async Task<bool> CanExecuteSemaphoreAsync()
        {
            if (!await _curationBackground.CanExecuteAsync())
            {
                _logger.LogWarning("Semaphore is red. Exiting execution.");
                return false;
            }
            return true;
        }

        public async Task<ActionResult> ExecuteAsync()
        {
            if (!await CanExecuteSemaphoreAsync())
            {
                _logger.LogWarning("Semaphore busy, execution cannot proceed.");
                return BadRequest("Semaphore busy, likely...");
            }

            var questionsToConfidence = await _questionRepository.GetRandomQuestionsToRateAsync(QuizId, 5, 5);
            if (questionsToConfidence == null || !questionsToConfidence.Any())
            {
                _logger.LogError("Questions not found.");
                return BadRequest("Questions not found.");
            }

            var resultJson = await _curationBackground.ExecuteAsync(questionsToConfidence);
            if (resultJson == null)
            {
                _logger.LogError("LLM didn't return a qualified string.");
                return BadRequest("LLM didn't return qualified string.");
            }

            if (!await SaveAiLog(resultJson))
            {
                _logger.LogError("Can't save AI log.");
                return BadRequest("Can't save AI log");
            }

            return await ProcessResultJson(resultJson);
        }

        private async Task<ActionResult> ProcessResultJson(string resultJson)
        {
            try
            {
                _logger.LogInformation($"Result JSON: {resultJson}");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Adjust as necessary
                };

                // Deserialize to a QuestionsCollection for better structure
                var questionsCollection = JsonSerializer.Deserialize<QuestionsCollection>(resultJson, options);
                if (questionsCollection == null || questionsCollection.Questions == null)
                {
                    _logger.LogError("Deserialization returned null.");
                    return BadRequest("Deserialization returned null.");
                }

                // Extract the list of questions from the dictionary
                var questionsAICuration = questionsCollection.Questions.Values.ToList();

                foreach (var question in questionsAICuration)
                {
                    _logger.LogInformation($"Processing Question ID: {question.Id}");
                    // You can perform additional processing here if needed
                }

                _logger.LogInformation("Questions successfully processed and deserialized.");
                return Ok(questionsAICuration); // Return the list if needed
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON deserialization error occurred. Invalid JSON format.");
                return BadRequest("Invalid JSON format.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing the result JSON.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        public class QuestionsCollection
        {
            public Dictionary<string, QuestionAICuration> Questions { get; set; } = new();
        }



        private async Task<bool> SaveAiLog(string resultJson)
        {
            try
            {
                await _aiLogRepository.CreateAILogAsync(new AILog
                {
                    Created = DateTime.UtcNow,
                    JSON = resultJson,
                    Name = "Questions / Curation / GetQuestionConfidency"
                });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the AI log.");
                return false;
            }
        }
    }
}
