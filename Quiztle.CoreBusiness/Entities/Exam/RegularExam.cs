using Quiztle.CoreBusiness.Entities.Quiz.DTO;

namespace Quiztle.CoreBusiness.Entities.Exam
{
    public class RegularExam
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<QuestionGameDTO> Questions { get; set; } = [];
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public int TotalQuestionsWithTrueAnswer()
        {
            int total = 0;

            foreach (var question in Questions)
            {
                if (question.Options.TryGetValue("Answer", out var option) && option.Item1)
                {
                    total++;
                }
            }

            return total;
        }

        public bool AllQuestionsAnsweredy()
        {
            return Questions.All(question => question.Options.Any(option => option.Value.Item1));
        }

        public int TotalQuestionsAnswered()
        {
            return Questions.Count(question => question.Options.Any(option => option.Value.Item1));
        }
    }
}
