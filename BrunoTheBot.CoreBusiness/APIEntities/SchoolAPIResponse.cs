using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class SchoolAPIResponse
    {
        public string Status { get; set; } = "";
        public School School { get; set; } = new();

    }
}
