using System.Linq;
using System.Text.Json.Serialization;

namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Response
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; } = new Guid();

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Shots")]
        public List<Shot> Shots { get; set; } = new List<Shot>();

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("Score")]
        public int Score { get; private set; } = 0;

        [JsonPropertyName("Percentage")]
        public decimal Percentage { get; private set; } = 0;

        [JsonPropertyName("IsFinalized")]
        public bool IsFinalized { get; private set; } = false;

        [JsonPropertyName("TestId")]
        public Guid TestId { get; set; } = Guid.NewGuid();

        //public void CalculateScore()
        //{
        //    if (IsFinalized)
        //        throw new InvalidOperationException("Response has already been finalized and cannot be modified.");

        //    Score = 0;
        //    foreach (var question in Test.Questions)
        //    {
        //        var correctOption = question.Options.FirstOrDefault(i => i.IsCorrect);
        //        if (correctOption != null && Shots.Any(s => s.Id == correctOption.Id))
        //        {
        //            Score++;
        //        }
        //    }

        //    CalculatePercentage();
        //    IsFinalized = true;
        //}

        //private void CalculatePercentage()
        //{
        //    if (Test.Questions.Count > 0)
        //    {
        //        Percentage = (decimal)Score / Test.Questions.Count * 100;
        //    }
        //    else
        //    {
        //        Percentage = 0;
        //    }
        //}
    }
}