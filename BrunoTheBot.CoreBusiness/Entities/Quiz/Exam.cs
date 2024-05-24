namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Exam
    {
        public Guid Id { get; set; }
        public List<Question> Questions { get; set; } = [];
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
