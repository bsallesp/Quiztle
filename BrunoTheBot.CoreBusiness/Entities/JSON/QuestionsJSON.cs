using System.Text.Json.Serialization;

public class QuestionsJson
{
    [JsonPropertyName("Questions")]
    public List<QuestionJson>? Questions { get; set; }
}
