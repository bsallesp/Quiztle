using Quiztle.CoreBusiness.Entities.Course;
using Quiztle.CoreBusiness.Entities.Quiz.DTO;
using Quiztle.CoreBusiness.Entities.Scratch;
using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Quiz
{
    public class Question
    {
        public Question()
        {
            Options = new List<Option>();
            Created = DateTime.UtcNow;
        }

        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Options")]
        public List<Option> Options { get; set; }

        [JsonPropertyName("Hint")]
        public string? Hint { get; set; } = "";

        [JsonPropertyName("Resolution")]
        public string? Resolution { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("DraftId")]
        public Guid? DraftId { get; set; }

        [JsonPropertyName("Draft")]
        public Draft? Draft { get; set; }

        [JsonPropertyName("TestId")]  // Adicione esta propriedade
        public Guid? TestId { get; set; }  // Chave estrangeira para o Teste

        [JsonPropertyName("Test")]  // Adicione esta navegação
        public Test? Test { get; set; }  // Navegação para a classe Test

        [JsonPropertyName("Verified")]
        public bool Verified { get; set; } = false;

        [JsonPropertyName("Consistency")]
        public bool Consistency { get; set; } = false;

        [JsonPropertyName("Rate")]
        public int Rate { get; set; } = 0;

        [JsonPropertyName("TestQuestions")]
        public ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();

        public Question GetQuestionSample() => QuestionFactory.CreateFilledQuestion();

        public List<Option> ShuffleOptions()
        {
            Shuffle(Options);
            return Options;
        }

        private static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public QuestionGameDTO ToQuestionGame()
        {
            var questionShape = new QuestionGameDTO
            {
                Id = this.Id,
                Question = this.Name,
                Options = new Dictionary<string, (bool, string)>()
            };

            int optionIndex = 1;
            foreach (var option in this.Options)
            {
                questionShape.Options.Add("Option_" + optionIndex++, (false, option.Name ?? ""));
            }

            return questionShape;
        }

        public string ToFormattedString()
        {
            var correctOption = Options.FirstOrDefault(o => o.IsCorrect);
            var formattedOptions = Options.Select(o =>
                $"{(o.IsCorrect ? "[Correct] " : "[Incorrect] ")}{o.Name}"
            );

            var questionString = $"{Name}\n";
            questionString += string.Join("\n", formattedOptions);

            return questionString;
        }
    }
}
