namespace Quiztle.CoreBusiness.Entities.Performance
{
    public class TestPerformance
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int UserId { get; set; } = 0;
        public string TestName { get; set; } = "";
        public List<QuestionsPerformance> QuestionsPerformance { get; set; } = [];
        public int CorrectAnswers { get; set; } = 0;
        public int IncorrectAnswers { get; set; } = 0;
        public int Score { get; set; } = 0;
    }
}
