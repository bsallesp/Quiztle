namespace Quiztle.CoreBusiness.Entities.Prompts
{
    public class Prompt
    {
        public Guid Id { get; set; } = new();
        public string? Text { get; set; }
        public DateTime Created = DateTime.UtcNow;
    }
}
