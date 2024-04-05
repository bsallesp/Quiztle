namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class Place
    {
        public int ID { get; set; }
        public string? Name { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
