using BrunoTheBot.CoreBusiness.CodeEntities;
using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class APIResponse<T>
    {
        [JsonPropertyName(nameof(Status))]
        public string Status { get; set; } = CustomStatusCodes.ErrorStatus;

        [JsonPropertyName(nameof(Data))]
        public required T Data { get; set; }

        [JsonPropertyName(nameof(Message))]
        public string? Message = "Unknown error.";

        public void SendError(string message)
        {
            this.Message = message;
            Status = CustomStatusCodes.ErrorStatus;
        }
    }
}
