namespace BrunoTheBot.CoreBusiness
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
