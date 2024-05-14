using BrunoTheBot.CoreBusiness.CodeEntities;
using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class APIResponse<T>
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = CustomStatusCodes.ErrorStatus;

        [JsonPropertyName("Data")]
        public required T Data { get; set; }

        [JsonPropertyName("Message")]
        public string? Message = "Unknown error.";

        public void SendError(string message)
        {
            this.Message = message;
            Status = CustomStatusCodes.ErrorStatus;
        }
    }
}
