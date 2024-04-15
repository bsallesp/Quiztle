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

            // Adiciona a resposta à lista
            shuffledList.Add(Answer!);

            // Adiciona todas as opções à lista
            foreach (var option in Options)
            {
                shuffledList.Add(option.Name);
            }

            // Embaralha a lista
            Shuffle(shuffledList);

            return shuffledList;
        }

        // Método de embaralhamento
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
    }
}
