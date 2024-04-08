namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class TopicClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public List<TopicClass> SubTopicClasses { get; set; } = [];
    }
}
