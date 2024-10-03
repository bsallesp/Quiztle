namespace BrunoTheBot.CoreBusiness
{
    public class AILog
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string JSON { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
