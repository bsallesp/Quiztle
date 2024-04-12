using BrunoTheBot.CoreBusiness.Entities.Quiz;

namespace BrunoTheBot.CoreBusiness.Entities.Exam
{
    public class Exam
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Question> Questions { get; set; } = [];
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
