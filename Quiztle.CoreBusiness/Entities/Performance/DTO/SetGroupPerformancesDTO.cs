using Quiztle.CoreBusiness.Entities.Performance;

namespace Quiztle.CoreBusiness.DTOs
{
    public class SetGroupedPerformancesDTO
    {
        public IEnumerable<GroupedTestPerformanceDTO> GroupedPerformances { get; set; } = [];

        public void GroupAndFill(IEnumerable<TestPerformance> testPerformances)
        {
            if (testPerformances == null || !testPerformances.Any())
                return;

            GroupedPerformances = testPerformances
                .GroupBy(tp => tp.TestId)
                .Select(group =>
                {
                    var groupedDTO = new GroupedTestPerformanceDTO
                    {
                        TestId = group.Key,
                        TestName = group.First().TestName,
                        Performances = group
                    };
                    return groupedDTO;
                });
        }

        public int GetTotalPerformances()
        {
            return GroupedPerformances.Sum(group => group.Performances.Count());
        }
    }
}
