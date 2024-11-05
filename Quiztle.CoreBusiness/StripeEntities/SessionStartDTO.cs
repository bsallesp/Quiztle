using System.Text.Json.Serialization;

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
}
