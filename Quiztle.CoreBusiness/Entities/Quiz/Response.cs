using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Quiz
{
    public class Response
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = new Guid();

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Shots")]
        public List<Shot> Shots { get; set; } = new List<Shot>();

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("Score")]
        public int Score { get; set; } = 0;

        [JsonPropertyName("Percentage")]
        public decimal Percentage { get; set; } = 0;

        [JsonPropertyName("IsFinalized")]
        public bool IsFinalized { get; set; } = false;

        [JsonPropertyName("TestId")]
        public Guid TestId { get; set; } = Guid.NewGuid();
    }
}