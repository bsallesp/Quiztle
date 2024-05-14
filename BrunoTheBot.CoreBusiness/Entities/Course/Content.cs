using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Course
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
