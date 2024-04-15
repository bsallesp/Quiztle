using System.Collections.Generic;
using System.Text.Json.Serialization;
using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class ChapterAPIResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("ChaptersAquired")]
        public List<Chapter> ChaptersAquired { get; set; } = new List<Chapter>();
    }
}
