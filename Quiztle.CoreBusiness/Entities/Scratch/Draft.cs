using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.CoreBusiness.Entities.Scratch
{
    public class Draft
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public List<Question>? Questions { get; set; } = new List<Question>();
        public int QuestionsAmountTarget { get; set; } = 10;

        public bool NeedMoreQuestions()
        {
            return Questions == null || Questions.Count < QuestionsAmountTarget;
        }

        public static Draft GetDraftSample()
        {
            return new Draft
            {
                Id = Guid.NewGuid(),
                Text = "Sample Draft",
                Questions = new List<Question> { QuestionFactory.CreateFilledQuestion() },
                QuestionsAmountTarget = 10
            };
        }
    }
}
