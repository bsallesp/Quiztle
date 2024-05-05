namespace BrunoTheBot.CoreBusiness.Entities.Tasks
{
    public class BookTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; } = "";
        public User? User { get; set; }
        public byte Status { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
