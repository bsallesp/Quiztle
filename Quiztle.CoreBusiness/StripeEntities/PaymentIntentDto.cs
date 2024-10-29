using System.Text.Json.Serialization;

public class PaymentIntentDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("amount_capturable")]
    public long? AmountCapturable { get; set; }

    [JsonPropertyName("amount_received")]
    public long AmountReceived { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("last_payment_error")]
    public string? LastPaymentError { get; set; }

    [JsonPropertyName("invoice")]
    public string? Invoice { get; set; }

    [JsonPropertyName("payment_method")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("payment_method_types")]
    public List<string>? PaymentMethodTypes { get; set; }

    [JsonPropertyName("livemode")]
    public bool Livemode { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("receipt_email")]
    public string? ReceiptEmail { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("next_action")]
    public object? NextAction { get; set; }
}
