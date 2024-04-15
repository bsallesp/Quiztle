using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class ContentAPIResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("NewContent")]
        public string NewContent { get; set; } = "";
    }
}
