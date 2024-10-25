public class AggregatedTagPerformance
{
    public string Tag { get; set; } = string.Empty;
    public int TotalCorrect { get; set; } = 0;
    public int TotalIncorrect { get; set; } = 0;

    public AggregatedTagPerformance(string tag)
    {
        Tag = tag;
    }
}
