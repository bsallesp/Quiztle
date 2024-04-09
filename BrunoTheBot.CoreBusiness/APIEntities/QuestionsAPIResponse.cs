using BrunoTheBot.CoreBusiness.Entities.Quiz;

namespace BrunoTheBot.CoreBusiness.APIEntities
{
    public class QuestionsAPIResponse
    {
        public string Status { get; set; } = "";
        public List<Question> Questions { get; set; } = [];
    }
}
