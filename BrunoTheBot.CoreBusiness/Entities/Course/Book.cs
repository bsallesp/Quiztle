using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class Book
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("Chapters")]
        public List<Chapter> Chapters { get; set; } = [];
    }
}
