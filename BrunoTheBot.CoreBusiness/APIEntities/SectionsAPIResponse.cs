using System.Collections.Generic;
using System.Text.Json.Serialization;
using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class SectionsAPIResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("SectionsAquired")]
        public List<Section> SectionsAquired { get; set; } = new List<Section>();
    }
}
