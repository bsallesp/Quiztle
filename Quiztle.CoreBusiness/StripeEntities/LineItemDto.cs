using System.Text.Json.Serialization;

public class LineItemDto
{
    [JsonPropertyName("price_id")]
    public string PriceId { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public long Quantity { get; set; }
}
