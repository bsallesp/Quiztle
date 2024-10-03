using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Course
{
    public class Book
    {
        [JsonPropertyName("Id")]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("Chapters")]
        public List<Chapter> Chapters { get; set; } = [];
        public int UserId { get; set; } = 0;
    }
}
