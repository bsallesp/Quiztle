using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.PDFData
{
    public class PDFDataPages
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("Page")]
        public int Page { get; set; }

        [JsonPropertyName("Content")]
        public string Content { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
