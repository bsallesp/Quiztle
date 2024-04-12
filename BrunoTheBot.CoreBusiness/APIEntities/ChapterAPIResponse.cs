using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class ChapterAPIResponse
    {
        public string Status { get; set; } = "";
        public List<Chapter> ChaptersAquired { get; set; } = [];
    }
}
