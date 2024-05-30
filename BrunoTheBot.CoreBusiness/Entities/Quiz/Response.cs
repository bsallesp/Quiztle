namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Response
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; } = "";
        public Test Test { get; set; } = new Test();
        public required Shot Shot { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
