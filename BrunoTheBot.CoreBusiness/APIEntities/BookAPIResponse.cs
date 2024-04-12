using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class BookAPIResponse
    {
        public string Status { get; set; } = "";
        public Book Book { get; set; } = new();

    }
}
