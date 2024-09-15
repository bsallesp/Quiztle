using Quiztle.CoreBusiness.Entities.Scratch;

namespace Quiztle.CoreBusiness.Entities.Prompts
{
    public class PromptItem
    {
        public Guid Id { get; set; } = new();
        public Guid PromptId { get; set; }
        public Prompt? Prompt { get; set; }
        public Guid? SentenceId { get; set; }
        public Sentence? Sentence { get; set; }
        public Guid? DraftId { get; set; }
        public Draft? Draft { get; set; }
        public int Order { get; set; } // Representa a ordem no prompt
    }
}
