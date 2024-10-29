using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PaidSessionDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("payment_status")]
    public string? PaymentStatus { get; set; }

    [JsonPropertyName("customer_email")]
    public string? CustomerEmail { get; set; }

    [JsonPropertyName("amount_total")]
    public long AmountTotal { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("success_url")]
    public string? SuccessUrl { get; set; }

    [JsonPropertyName("cancel_url")]
    public string? CancelUrl { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("payment_intent_id")]
    public string? PaymentIntentId { get; set; }

    [JsonPropertyName("line_items")]
    public List<LineItemDto> LineItems { get; set; } = new List<LineItemDto>();
}
