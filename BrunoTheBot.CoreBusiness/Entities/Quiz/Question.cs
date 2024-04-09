namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Question
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Answer { get; set; } = "";
        public List<Option> Options { get; set; } = [];
        public string? Hint { get; set; } = "";
        public DateTime Created { get; set; }
    }
}
