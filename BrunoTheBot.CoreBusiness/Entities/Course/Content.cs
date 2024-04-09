using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class Content
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Text")]
        public string? Text { get; set; } = "";
    }
}
