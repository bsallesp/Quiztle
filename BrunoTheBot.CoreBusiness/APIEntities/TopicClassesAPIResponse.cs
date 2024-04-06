using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class TopicClassesAPIResponse
    {
        public string Status { get; set; } = "";
        public List<TopicClass> TopicClasses { get; set; } = [];

    }
}
