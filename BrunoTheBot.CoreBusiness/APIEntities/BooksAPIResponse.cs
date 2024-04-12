using BrunoTheBot.CoreBusiness.Entities.Course;
using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class BooksAPIResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = "";
        [JsonPropertyName("Books")]
        public List<Book>? Books { get; set; }
    }
}
