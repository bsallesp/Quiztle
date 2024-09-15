namespace Quiztle.CoreBusiness.Entities.Prompts
{
    public class Prompt
    {
        public Guid Id { get; set; } = new();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public ICollection<PromptItem> Items { get; set; } = [];
    }
}
