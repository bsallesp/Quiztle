namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Question
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; }
        public List<Option> Options { get; set; } = new List<Option>();
        public Answer Answer { get; set; } = new Answer();
        public string? Hint { get; set; } = "";
    }
}
