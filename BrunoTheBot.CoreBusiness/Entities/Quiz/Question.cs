using BrunoTheBot.CoreBusiness.Entities.Quiz.DTO;
using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Question
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Answer")]
        public string? Answer { get; set; } = "";

        [JsonPropertyName("Options")]
        public List<Option> Options { get; set; } = new List<Option>();

        [JsonPropertyName("Hint")]
        public string? Hint { get; set; } = "";

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public List<string> GetShuffledAnswerAndOptions()
        {
            List<string> shuffledList = new List<string>();

            shuffledList.Add(Answer!);

            foreach (var option in Options)
            {
                shuffledList.Add(option.Name);
            }

            Shuffle(shuffledList);

            return shuffledList;
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

            questionShape.Options.Add("Answer", (true, this.Answer ?? ""));

            int optionIndex = 1;
            foreach (var option in this.Options)
            {
                questionShape.Options.Add("Option_" + optionIndex++, (false, option.Name ?? ""));
            }

            return questionShape;
        }
    }
}
