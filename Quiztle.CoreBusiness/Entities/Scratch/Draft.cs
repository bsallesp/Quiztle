using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.CoreBusiness.Entities.Scratch
{
    public class Draft
    {
        public Guid Id { get; set; }
        public string? Title { get; set; } = "";
        public string? OriginalContent { get; set; } = "";
        public string? MadeByAiContent { get; set; } = "";
        public string? Tag { get; set; } = "";
        public List<Question>? Questions { get; set; } = [];
        public int QuestionsAmountTarget { get; set; } = 10;

        public Draft()
        {

        }

        public bool IsNeededMoreQuestions()
        {
            return Questions == null || Questions.Count < QuestionsAmountTarget;
        }

        public List<Question> GetRandomQuestions(int amount)
        {
            if (Questions == null || Questions.Count == 0)
                return [];

            var availableAmount = Math.Min(amount, Questions.Count);
            var random = new Random();

            return Questions
                .OrderBy(q => random.Next())
                .Take(availableAmount)
                .ToList();
        }

        public static Draft GetDraftSample()
        {
            return new Draft
            {
                Id = Guid.NewGuid(),
                OriginalContent = "Sample Draft",
                Questions = new List<Question> { QuestionFactory.CreateFilledQuestion() },
                QuestionsAmountTarget = 10
            };
        }
    }
}
