using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Shot
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("OptionId")]
        public Guid OptionId { get; set; }

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("ResponseId")]
        public Guid ResponseId { get; set; }
    }
}
