using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Paid
{
    public class Paid
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = new();


        [JsonPropertyName("UserId")]
        public Guid? UserId { get; set; }


        [JsonPropertyName("CustomerId")]
        public string? CustomerId { get; set; }


        [JsonPropertyName("UserEmail")]
        public string? UserEmail { get; set; }


        [JsonPropertyName("PriceId")]
        public string? PriceId { get; set; }


        [JsonPropertyName("TestId")]
        public string? TestId { get; set; }


        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;


        [JsonPropertyName("Amount")]
        public int Amount { get; set; }


        [JsonPropertyName("Currency")]
        public string Currency { get; set; } = "USD";


        [JsonPropertyName("Status")]
        public string Status { get; set; } = "Pending";


        [JsonPropertyName("Description")]
        public string? Description { get; set; }


        [JsonPropertyName("PaymentMethod")]
        public string? PaymentMethod { get; set; }


        [JsonPropertyName("PaymentIntentId")]
        public string? PaymentIntentId { get; set; }


        [JsonPropertyName("LastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        public bool IsValid() =>
            !string.IsNullOrWhiteSpace(TestId) &&
            !string.IsNullOrWhiteSpace(PriceId) &&
            Amount > 0 &&
            !string.IsNullOrWhiteSpace(UserEmail);

        public string ToJson() => JsonSerializer.Serialize(this);
        
        public SessionStartDTO ToSessionStartDTO() => new SessionStartDTO
        {
            ClientId = UserId?.ToString(),
            CustomerId = CustomerId,
            Amount = Amount,
            Email = UserEmail,
            PriceId = PriceId,
            TestId = TestId,
            Description = Description,
            Created = Created
        };

    }
}
