using System.Text.Json.Serialization;
using Quiztle.CoreBusiness.Entities.Paid;

public class SessionStartDTO
{
    [JsonPropertyName("ClientId")]
    public string? ClientId { get; set; }
    
    [JsonPropertyName("CustomerId")]
    public string? CustomerId { get; set; }
    
    [JsonPropertyName("Amount")]
    public int Amount { get; set; }

    [JsonPropertyName("Email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("PriceId")]
    public string? PriceId { get; set; }

    [JsonPropertyName("TestId")]
    public string? TestId { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("Created")]
    public DateTime Created { get; set; }
    
    public static SessionStartDTO FromPaid(Paid paid) => new SessionStartDTO
    {
        ClientId = paid.UserId?.ToString(),
        CustomerId = paid.CustomerId,
        Amount = paid.Amount,
        Email = paid.UserEmail,
        PriceId = paid.PriceId,
        TestId = paid.TestId,
        Description = paid.Description,
        Created = paid.Created
    };
}
