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

        public List<Question> GetRandomQuestions(int amount)
        {
            if (Questions == null || !Questions.Any())
                return new List<Question>();

            var random = new Random();
            return Questions
                .OrderBy(q => random.Next()) // Ordena aleatoriamente
                .Take(amount) // Seleciona a quantidade desejada
                .ToList();
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
