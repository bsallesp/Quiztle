using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Course
{
    public class Content
    {
        [JsonPropertyName("Id")]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("Text")]
        public string? Text { get; set; } = "";
    }
}
