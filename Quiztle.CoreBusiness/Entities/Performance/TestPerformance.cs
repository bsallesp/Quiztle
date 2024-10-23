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
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
