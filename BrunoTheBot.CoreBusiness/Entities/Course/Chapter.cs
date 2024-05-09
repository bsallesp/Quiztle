using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class Chapter
    {
        [JsonPropertyName("Id")]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("Sections")]
        public List<Section> Sections { get; set; } = new List<Section>();
    }
}
