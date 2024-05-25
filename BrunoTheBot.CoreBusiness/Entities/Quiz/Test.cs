using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Test
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Questions")]
        public List<Question> Questions { get; set; } = [];

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("PDFDataId")]
        public Guid PDFDataId { get; set; }
    }
}
