using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Shot
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("Option")]
        public Option Option { get; set; } = new Option();

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
