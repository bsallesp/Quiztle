using Quiztle.CoreBusiness.Entities.Performance.DTO;
using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Performance
{
    public class QuestionsPerformance
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("QuestionName")]
        public string QuestionName { get; set; } = "";

        [JsonPropertyName("CorrectAnswerName")]
        public string CorrectAnswerName { get; set; } = "";

        [JsonPropertyName("IncorrectAnswerName")]
        public string IncorrectAnswerName { get; set; } = "";

        [JsonPropertyName("TagName")]
        public string TagName { get; set; } = "";
    }
}
