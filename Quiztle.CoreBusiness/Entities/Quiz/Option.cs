using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Quiz
{
    public class Option
    {

        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("IsCorrect")]
        public bool IsCorrect { get; set; } = false;

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("QuestionId")]
        public Guid QuestionId { get; set; }

        [JsonIgnore]
        public Question? Question { get; set; }
    }
}
