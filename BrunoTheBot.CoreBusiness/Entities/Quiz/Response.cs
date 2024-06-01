namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Response
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; } = "";
        public Test Test { get; set; } = new Test();
        public Shot Shot { get; set; } = new Shot();
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
