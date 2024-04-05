using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.DataContext
{
    public class TopicSeed
    {
        private string RandomOne(string[] list)
        {
            var idx = new Random().Next(list.Length);
            return list[idx];
        }

        private TopicClass MakeTopic()
        {
            var topic = new TopicClass
            {
                Name = RandomOne(_names),
                Created = DateTime.Now
            };
            return topic;
        }

        public async Task SeedDatabaseWithTopicCountAsync(PostgreBrunoTheBotContext context, int totalCount)
        {
            var count = 0;
            var currentCycle = 0;
            while (count < totalCount)
            {
                var list = new List<TopicClass>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(MakeTopic());
                }
                if (list.Count > 0)
                {
                    context.TopicClasses?.AddRange(list);
                    await context.SaveChangesAsync();
                }
                currentCycle = 0;
            }
        }

        public Task<List<TopicClass>> GetTopicsAsync(int totalCount)
        {
            var count = 0;
            var currentCycle = 0;
            var topics = new List<TopicClass>();

            while (count < totalCount)
            {
                var list = new List<TopicClass>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(MakeTopic());
                }
                if (list.Count > 0)
                {
                    topics.AddRange(list);
                }
                currentCycle = 0;
            }

            return Task.FromResult(topics);
        }

        private readonly string[] _names = new[]
        {
            "Topic 1",
            "Topic 2",
            "Topic 3",
            "Topic 4",
            "Topic 5",
            "Topic 6",
            "Topic 7",
            "Topic 8",
            "Topic 9",
            "Topic 10",
            "Topic 11",
            "Topic 12",
            "Topic 13",
            "Topic 14",
            "Topic 15",
            "Topic 16",
            "Topic 17",
            "Topic 18",
            "Topic 19",
            "Topic 20",
            "Topic 21",
            "Topic 22",
            "Topic 23",
            "Topic 24",
            "Topic 25",
            "Topic 26",
            "Topic 27",
            "Topic 28",
            "Topic 29",
            "Topic 30",
            "Topic 31",
            "Topic 32",
            "Topic 33",
            "Topic 34",
            "Topic 35",
            "Topic 36",
            "Topic 37",
            "Topic 38",
            "Topic 39",
            "Topic 40",
            "Topic 41",
            "Topic 42",
            "Topic 43",
            "Topic 44",
            "Topic 45",
            "Topic 46",
            "Topic 47",
            "Topic 48",
            "Topic 49",
            "Topic 50",
            "Topic 51",
            "Topic 52",
            "Topic 53",
            "Topic 54",
            "Topic 55",
            "Topic 56",
            "Topic 57",
            "Topic 58",
            "Topic 59",
            "Topic 60",
            "Topic 61",
            "Topic 62",
            "Topic 63",
            "Topic 64",
            "Topic 65",
            "Topic 66",
            "Topic 67",
            "Topic 68",
            "Topic 69",
            "Topic 70",
            "Topic 71",
            "Topic 72",
            "Topic 73",
            "Topic 74",
            "Topic 75",
            "Topic 76",
            "Topic 77",
            "Topic 78",
            "Topic 79",
            "Topic 80",
            "Topic 81",
            "Topic 82",
            "Topic 83",
            "Topic 84",
            "Topic 85",
            "Topic 86",
            "Topic 87",
            "Topic 88",
            "Topic 89",
            "Topic 90",
            "Topic 91",
            "Topic 92",
            "Topic 93",
            "Topic 94",
            "Topic 95",
            "Topic 96",
            "Topic 97",
            "Topic 98",
            "Topic 99",
            "Topic 100"
        };
    }
}
