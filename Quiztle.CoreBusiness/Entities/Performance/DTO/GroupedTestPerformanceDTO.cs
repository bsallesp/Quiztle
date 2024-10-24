using Quiztle.CoreBusiness.Entities.Performance;

namespace Quiztle.CoreBusiness.DTOs
{
    public class GroupedTestPerformanceDTO
    {
        public Guid TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public IEnumerable<TestPerformance> Performances { get; set; } = [];

        public void GroupAndFill(IEnumerable<TestPerformance> testPerformances)
        {
            if (testPerformances == null || !testPerformances.Any())
                return;

            var groupedPerformances = testPerformances.GroupBy(tp => tp.TestId).FirstOrDefault();

            if (groupedPerformances != null)
            {
                TestId = groupedPerformances.Key;
                TestName = groupedPerformances.First().TestName;
                Performances = groupedPerformances;
            }
        }
    }
}
