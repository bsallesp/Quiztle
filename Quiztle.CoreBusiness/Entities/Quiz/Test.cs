using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Quiz
{
    public class Test
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("QuestionsDTO")]
        public List<Question> Questions { get; set; } = new List<Question>();

        [JsonPropertyName("Responses")]
        public List<Response>? Responses { get; set; } = [];

        [JsonPropertyName("PDFDataId")]
        public Guid? PDFDataId { get; set; } = null; // Inicialize com null

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

        public Question GetRandomQuestion()
        {
            if (Questions == null || Questions.Count == 0)
            {
                throw new InvalidOperationException("Não há perguntas disponíveis.");
            }

            Random rng = new Random();
            int index = rng.Next(Questions.Count);
            return Questions[index];
        }

    }
}
