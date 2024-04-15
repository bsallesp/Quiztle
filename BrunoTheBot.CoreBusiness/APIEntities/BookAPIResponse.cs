using System.Text.Json.Serialization;
using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class BookAPIResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("Book")]
        public Book Book { get; set; } = new();
    }
}
