
namespace Quiztle.CoreBusiness.Entities.Performance
{
    public class QuestionsPerformance
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string QuestionName { get; set; } = "";
        public string CorrectAnswerName { get; set; } = "";
        public string IncorrectAnswerName { get; set; } = "";
        public string TagName { get; set; } = "";
    }
}
