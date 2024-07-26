using System.Text.Json.Serialization;

public class QuestionsJson
{
    [JsonPropertyName("QuestionsDTO")]
    public List<QuestionJson>? Questions { get; set; }
}
