namespace BrunoTheBot.CoreBusiness.Entities
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created { get; set; }
        public List<Topic> Topics { get; set; } = new List<Topic>();
    }
}
