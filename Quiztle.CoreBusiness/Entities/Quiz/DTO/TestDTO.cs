using Quiztle.CoreBusiness.Entities.Performance;
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
        public string ShieldSVG = "";
        public void FromTest(Test test)
        {
            Id = test.Id;
            Name = test.Name;
            QuestionsDTO = test.Questions.Select(q => new QuestionDTO(q)).ToList();
            //PDFDataId = test.PDFDataId;
            Created = test.Created;
            ShieldSVG = test.ShieldSVG!;
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

        public int GetTotalSelectedOrFinishedQuestions()
        {
            int totalSelectedOrFinished = 0;

            foreach (var questionDTO in QuestionsDTO)
            {
                if (questionDTO.OptionsDTO.Any(o => o.IsSelected) || questionDTO.IsFinished)
                {
                    totalSelectedOrFinished++;
                }
            }

            return totalSelectedOrFinished;
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

        public void SetQuestionsAmount(int amount)
        {
            if (amount >= QuestionsDTO.Count)
            {
                throw new ArgumentException("The amount must be less than the number of questions.");
            }

            Random rng = new Random();
            var indicesToKeep = new HashSet<int>();

            while (indicesToKeep.Count < amount)
            {
                indicesToKeep.Add(rng.Next(QuestionsDTO.Count));
            }
            QuestionsDTO = QuestionsDTO
                .Select((q, index) => new { q, index })
                .Where(x => indicesToKeep.Contains(x.index))
                .Select(x => x.q)
                .ToList();
        }

        public int GetTotalFinishedQuestions()
        {
            return QuestionsDTO.Count(q => q.IsFinished);
        }

        public TestPerformance CreateTestPerformance(string userId)
        {
            var testPerformance = new TestPerformance
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                TestName = this.Name,
                CorrectAnswers = this.GetTotalCorrectAnswers(),
                IncorrectAnswers = this.QuestionsDTO.Count - this.GetTotalCorrectAnswers(),
                Score = this.CalculateScore(),
                QuestionsPerformance = [],
                TestId = Id
            };

            foreach (var questionDTO in QuestionsDTO)
            {
                var questionPerformance = new QuestionsPerformance
                {
                    Id = Guid.NewGuid(),
                    QuestionName = questionDTO.Name,
                    CorrectAnswerName = string.Join(", ", questionDTO.OptionsDTO.Where(o => o.IsCorrect).Select(o => o.Name)),
                    IncorrectAnswerName = string.Join(", ", questionDTO.OptionsDTO.Where(o => !o.IsCorrect && o.IsSelected).Select(o => o.Name)),
                    TagName = questionDTO.Tag
                };

                testPerformance.QuestionsPerformance.Add(questionPerformance);
            }

            return testPerformance;
        }


        private int CalculateScore()
        {
            int totalQuestions = this.QuestionsDTO.Count;
            int correctAnswers = this.GetTotalCorrectAnswers();

            if (totalQuestions == 0)
            {
                return 0;
            }

            return correctAnswers * 100 / totalQuestions;
        }
    }
}