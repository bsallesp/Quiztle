using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness.Entities.Quiz
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
        public List<Response>? Responses { get; set; } = new List<Response>();
        
        [JsonPropertyName("PDFDataId")]
        public Guid? PDFDataId { get; set; } = null;

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("IsAvaiable")]
        public bool IsAvaiable { get; set; } = false;

        [JsonPropertyName("IsPremium")]
        public bool IsPremium { get; set; } = false;
        
        [JsonPropertyName("TestQuestions")]
        public ICollection<TestQuestion> TestQuestions { get; set; } = [];
        
        [JsonPropertyName("PriceId")]
        public string? PriceId { get; set; } = "";
        
        [JsonPropertyName("PriceInCents")]
        public int PriceInCents { get; set; } = 0;
        
        [JsonPropertyName("ShieldSVG")]
        public string? ShieldSVG { get; set; } = "default.svg";

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
        
        public Dictionary<string, int> GetQuestionCountByTag()
        {
            return Questions
                .GroupBy(q => q.Tag ?? "Undefined")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public string?[] GetTagsSet() => Questions.Select(t => t.Tag).Distinct().ToArray();

        public Test GetFreeTest(int countPerTag)
        {
            var selectedQuestions = Questions
                .GroupBy(q => q.Tag ?? "Undefined")
                .SelectMany(g => g.Take(countPerTag))
                .ToList();

            return new Test
            {
                Id = Guid.NewGuid(),
                Name = Name + " FreeTest",
                Questions = selectedQuestions,
                Created = DateTime.UtcNow
            };
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

        public static Test GetTestSample()
        {
            return new Test
            {
                Id = Guid.NewGuid(),
                Name = "Sample Test",
                Questions = new List<Question> { QuestionFactory.CreateFilledQuestion() },
                Created = DateTime.UtcNow
            };
        }
    }
}