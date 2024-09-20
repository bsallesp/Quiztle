namespace Quiztle.CoreBusiness.Entities.Quiz
{
    public class TestQuestion
    {
        public Guid TestId { get; set; }
        public required Test Test { get; set; }
        public Guid QuestionId { get; set; }
        public required Question Question { get; set; }
    }
}
