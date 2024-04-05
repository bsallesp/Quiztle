using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Question
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; }
        public List<Option> Options { get; set; } = [];
        public Answer Answer { get; set; } = new Answer();
        public TopicClass Topic { get; set; } = new TopicClass();
        public string? Hint { get; set; } = "";
    }
}
