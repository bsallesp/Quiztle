namespace BrunoTheBot.CoreBusiness.Entities.Quiz.DTO
{
    public class ResponseSheet
    {
        public int Id { get; set; }
        public List<QuestionQuiz> QuestionsQuiz { get; set; } = new List<QuestionQuiz>();
        public int Hits { get; set; }

        public ResponseSheet()
        {

        }

        public void CalculateHits()
        {
            foreach (var questionQuiz in QuestionsQuiz)
            {
                if (questionQuiz.Options.TryGetValue("Answer", out var answer) && answer.Item1)
                {
                    Hits++;
                }
            }
        }
    }
}
