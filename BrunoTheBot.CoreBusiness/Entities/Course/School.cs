namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public List<TopicClass> Topics { get; set; } = [];
    }
}
