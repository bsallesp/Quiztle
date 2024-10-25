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

        public int TotalHits()
        {
            return Performances.Sum(tp => tp.CorrectAnswers);
        }

        public int TotalMiss()
        {
            return Performances.Sum(tp => tp.IncorrectAnswers);
        }

        public int TotalProbla() => TotalHits() + TotalMiss();

        public double MediaScore()
        {
            if (!Performances.Any())
                return 0;

            return Performances.Average(tp => tp.Score);
        }

        public IEnumerable<AggregatedTagPerformance> GetAggregatedPerformanceByTags()
        {
            var allAggregatedPerformances = new List<AggregatedTagPerformance>();

            foreach (var performance in Performances)
            {
                var aggregatedPerformances = performance.AggregatePerformanceByTags();

                foreach (var aggregated in aggregatedPerformances)
                {
                    var existingTag = allAggregatedPerformances.FirstOrDefault(atp => atp.Tag == aggregated.Tag);
                    if (existingTag != null)
                    {
                        existingTag.TotalCorrect += aggregated.TotalCorrect;
                        existingTag.TotalIncorrect += aggregated.TotalIncorrect;
                    }
                    else
                    {
                        allAggregatedPerformances.Add(new AggregatedTagPerformance(aggregated.Tag)
                        {
                            TotalCorrect = aggregated.TotalCorrect,
                            TotalIncorrect = aggregated.TotalIncorrect
                        });
                    }
                }
            }

            return allAggregatedPerformances;
        }

    }
}