using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.CoreBusiness.Entities.Course
{
    public class Section
    {
        [JsonPropertyName("Id")]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("Content")]
        public Content Content { get; set; } = new Content();

        [JsonPropertyName("Questions")]
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
