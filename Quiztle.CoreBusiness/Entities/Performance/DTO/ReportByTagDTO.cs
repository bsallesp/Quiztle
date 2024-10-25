namespace Quiztle.CoreBusiness.Entities.Performance.DTO
{
    public class ReportByTagDTO
    {
        public string? Tag { get; set; }
        public int CorrectAmount { get; set; }
        public int IncorrectAmount { get; set; }

        public int GetTotalAttempts()
        {
            return CorrectAmount + IncorrectAmount;
        }

        public double GetCorrectPercentage()
        {
            int totalAttempts = GetTotalAttempts();
            return totalAttempts > 0 ? (double)CorrectAmount / totalAttempts * 100 : 0;
        }

        public double GetIncorrectPercentage()
        {
            int totalAttempts = GetTotalAttempts();
            return totalAttempts > 0 ? (double)IncorrectAmount / totalAttempts * 100 : 0;
        }
    }
}
