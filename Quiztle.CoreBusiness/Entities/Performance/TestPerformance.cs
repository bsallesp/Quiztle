﻿using Quiztle.CoreBusiness.Entities.Performance.DTO;
using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Performance
{
    public class TestPerformance
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonPropertyName("UserId")]
        public Guid UserId { get; set; } = Guid.NewGuid();
        [JsonPropertyName("TestName")]
        public string TestName { get; set; } = "";
        [JsonPropertyName("QuestionsPerformance")]
        public List<QuestionsPerformance> QuestionsPerformance { get; set; } = [];
        [JsonPropertyName("CorrectAnswers")]
        public int CorrectAnswers { get; set; } = 0;
        [JsonPropertyName("IncorrectAnswers")]
        public int IncorrectAnswers { get; set; } = 0;
        [JsonPropertyName("TestId")]
        public Guid TestId { get; set; } = new Guid();

        [JsonPropertyName("Score")]
        public int Score { get; set; } = 0;
        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        [JsonPropertyName("Shield")]
        public string Shield { get; set; } = "default.svg";

        public IEnumerable<ReportByTagDTO> ReportsByTag()
        {
            return QuestionsPerformance
                .GroupBy(q => q.TagName)
                .Select(g => new ReportByTagDTO
                {
                    Tag = g.Key,
                    CorrectAmount = g.Count(q => q.CorrectAnswerName != ""),
                    IncorrectAmount = g.Count(q => q.IncorrectAnswerName != "")
                });
        }

        public IEnumerable<AggregatedTagPerformance> AggregatePerformanceByTags()
        {
            return QuestionsPerformance
                .GroupBy(q => q.TagName)
                .Select(g => new AggregatedTagPerformance(g.Key)
                {
                    TotalCorrect = g.Sum(q => q.CorrectAnswerName != "" ? 1 : 0),
                    TotalIncorrect = g.Sum(q => q.IncorrectAnswerName != "" ? 1 : 0)
                });
        }

        public int TotalOptions() => CorrectAnswers + IncorrectAnswers;

    }
}
