using BrunoTheBot.CoreBusiness.Entities.Course;
using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class SchoolsAPIResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = "";
        [JsonPropertyName("Schools")]
        public List<School>? Schools { get; set; }
    }
}
