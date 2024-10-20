using System.Text.Json.Serialization;

public class QuestionAICuration
{
    /// <summary>
    /// Gets or sets the list of options for the question.
    /// </summary>
    [JsonPropertyName("OptionsDTO")]
    public List<OptionAICurationDTO>? OptionsDTO { get; private set; }

    /// <summary>
    /// Gets or sets the unique identifier for the question.
    /// </summary>
    [JsonPropertyName("Id")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the GUID identifier for additional identification purposes.
    /// </summary>
    public Guid? GuidId { get; private set; }

    public QuestionAICuration(string id, List<OptionAICurationDTO>? optionsDTO, Guid? guidId = null)
    {
        Id = id;
        OptionsDTO = optionsDTO ?? [];
        GuidId = guidId;

        ValidateOptions(); // Validate upon creation
    }

    /// <summary>
    /// Validates the options provided for the question.
    /// </summary>
    public void ValidateOptions()
    {
        if (OptionsDTO == null || !OptionsDTO.Any())
        {
            throw new ArgumentException("OptionsDTO must contain at least one option.");
        }

        if (!OptionsDTO.Any(option => option.IsCorrect))
        {
            throw new ArgumentException("At least one option must be marked as correct.");
        }
    }

    public override string ToString()
    {
        return $"{Id}: {OptionsDTO?.Count ?? 0} options";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not QuestionAICuration other) return false;
        return Id == other.Id &&
               (OptionsDTO?.SequenceEqual(other.OptionsDTO!) ?? other.OptionsDTO == null);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, OptionsDTO);
    }
}
