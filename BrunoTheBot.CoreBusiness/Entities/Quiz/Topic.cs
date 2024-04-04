namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Reference> References { get; set; } = new List<Reference>();
    }
}
