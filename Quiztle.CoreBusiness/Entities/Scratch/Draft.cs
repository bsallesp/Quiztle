using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.CoreBusiness.Entities.Scratch
{
    public class Draft
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public List<Question>? Questions { get; set; }
    }
}
