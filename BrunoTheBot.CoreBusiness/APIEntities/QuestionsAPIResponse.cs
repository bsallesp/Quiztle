using System.Collections.Generic;
using System.Text.Json.Serialization;
using BrunoTheBot.CoreBusiness.Entities.Quiz;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class QuestionsAPIResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("Questions")]
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
