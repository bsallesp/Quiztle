using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class Chapter
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("Sections")]
        public List<Section> Sections { get; set; } = new List<Section>();
    }
}
