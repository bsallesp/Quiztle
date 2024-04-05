namespace BrunoTheBot.CoreBusiness.Entities.Course
{
    public class Content
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Place>? Places { get; set; }
    }
}
