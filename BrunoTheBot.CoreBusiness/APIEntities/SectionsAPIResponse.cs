using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class SectionsAPIResponse
    {
        public string Status { get; set; } = "";
        public List<Section> SectionsAquired { get; set; } = [];
    }
}