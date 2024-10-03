using System.Text.Json.Serialization;

public class QuestionJson
{
    [JsonPropertyName("Question")]
    public string? Question { get; set; }

    [JsonPropertyName("Answer")]
    public string? Answer { get; set; }

    [JsonPropertyName("Option1")]
    public string? Option1 { get; set; }

    [JsonPropertyName("Option2")]
    public string? Option2 { get; set; }

    [JsonPropertyName("Option3")]
    public string? Option3 { get; set; }

    [JsonPropertyName("Option4")]
    public string? Option4 { get; set; }

    [JsonPropertyName("Hint")]
    public string? Hint { get; set; }

    [JsonPropertyName("Resolution")]
    public string? Resolution { get; set; }
}