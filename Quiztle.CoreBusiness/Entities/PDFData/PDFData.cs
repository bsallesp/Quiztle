using System.Text.Json.Serialization;
using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.CoreBusiness.Entities.PDFData
{
    public class PDFData
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("FileName")]
        public string FileName { get; set; } = "";

        [JsonPropertyName("Pages")]
        public List<PDFDataPages> Pages { get; set; } = [];

        [JsonPropertyName("Tests")]
        public List<Test>? Tests { get; set; } = [];

        [JsonPropertyName("Description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}