using Quiztle.CoreBusiness.Entities.Quiz.DTO;

namespace Quiztle.CoreBusiness.Entities.Quiz
{
    public class TestDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public List<QuestionDTO> QuestionsDTO { get; set; } = new List<QuestionDTO>();

        public Guid PDFDataId { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public int TotalCorrectAnswers = 0;

        public void FromTest(Test test)
        {
            Id = test.Id;
            Name = test.Name;
            QuestionsDTO = test.Questions.Select(q => new QuestionDTO(q)).ToList();
            //PDFDataId = test.PDFDataId;
            Created = test.Created;
        }

        public void ShuffleQuestions()
        {
            Random rng = new Random();
            int n = QuestionsDTO.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                QuestionDTO value = QuestionsDTO[k];
                QuestionsDTO[k] = QuestionsDTO[n];
                QuestionsDTO[n] = value;
            }
        }

        public int GetTotalSelectedAnswers()
        {
            var totalSelectedAnswers = 0;

            foreach (var questionDTO in QuestionsDTO)
            {
                if (questionDTO.OptionsDTO.Any(o => o.IsSelected)) totalSelectedAnswers++;
            }

            return totalSelectedAnswers;
        }

        public int GetTotalCorrectAnswers()
        {
            TotalCorrectAnswers = 0;

            foreach (var questionDTO in QuestionsDTO)
            {
                if (questionDTO.OptionsDTO.Count(o => o.IsCorrect) ==
                    questionDTO.OptionsDTO.Count(o => o.IsCorrect && o.IsSelected))
                {
                    TotalCorrectAnswers++;
                }
            }

            return TotalCorrectAnswers;
        }
    }
}
