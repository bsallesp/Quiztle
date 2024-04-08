namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public Content Content { get; set; } = new();
    }
}
