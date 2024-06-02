using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Test
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("Questions")]
        public List<Question> Questions { get; set; } = new List<Question>();

        [JsonPropertyName("Responses")]
        public List<Response>? Responses { get; set; } = [];

        [JsonPropertyName("PDFDataId")]
        public Guid PDFDataId { get; set; }

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public void ShuffleQuestions()
        {
            Random rng = new Random();
            int n = Questions.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Question value = Questions[k];
                Questions[k] = Questions[n];
                Questions[n] = value;
            }
        }
    }
}
