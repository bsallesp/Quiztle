using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class SchoolsAPIResponse
    {
        public string Status { get; set; } = "";
        public IQueryable<School>? Schools { get; set; }

    }
}
