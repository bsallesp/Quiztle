using System.Text.Json.Serialization;

/// <summary>
/// Represents an option for a question, including its correctness.
/// </summary>
public class OptionAICurationDTO
{
    /// <summary>
    /// Gets or sets the unique identifier for the option.
    /// </summary>
    [JsonPropertyName("Id")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this option is the correct answer.
    /// </summary>
    [JsonPropertyName("IsCorrect")]
    public bool IsCorrect { get; private set; }

    public OptionAICurationDTO(string id, bool isCorrect)
    {
        Id = id;
        IsCorrect = isCorrect;
    }

    public override string ToString()
    {
        return $"{Id}: IsCorrect = {IsCorrect}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not OptionAICurationDTO other) return false;
        return Id == other.Id && IsCorrect == other.IsCorrect;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, IsCorrect);
    }
}
