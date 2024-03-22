using BrunoTheBot.CoreBusiness;

namespace BrunoTheBot.DataContext
{
    public class AnswerSeed
    {
        private string RandomOne(string[] list)
        {
            var idx = new Random().Next(list.Length);
            return list[idx];
        }

        private Answer MakeAnswer()
        {
            var answer = new Answer
            {
                Name = RandomOne(_names),
                Created = DateTime.Now
            };
            return answer;
        }

        public async Task SeedDatabaseWithAnswerCountAsync(SqliteDataContext context, int totalCount)
        {
            var count = 0;
            var currentCycle = 0;
            while (count < totalCount)
            {
                var list = new List<Answer>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(MakeAnswer());
                }
                if (list.Count > 0)
                {
                    context.Answers?.AddRange(list);
                    await context.SaveChangesAsync();
                }
                currentCycle = 0;
            }
        }

        private readonly string[] _names = new[]
        {
            "Answer 1",
            "Answer 2",
            "Answer 3",
            "Answer 4",
            "Answer 5",
            "Answer 6",
            "Answer 7",
            "Answer 8",
            "Answer 9",
            "Answer 10",
            "Answer 11",
            "Answer 12",
            "Answer 13",
            "Answer 14",
            "Answer 15",
            "Answer 16",
            "Answer 17",
            "Answer 18",
            "Answer 19",
            "Answer 20",
            "Answer 21",
            "Answer 22",
            "Answer 23",
            "Answer 24",
            "Answer 25",
            "Answer 26",
            "Answer 27",
            "Answer 28",
            "Answer 29",
            "Answer 30",
            "Answer 31",
            "Answer 32",
            "Answer 33",
            "Answer 34",
            "Answer 35",
            "Answer 36",
            "Answer 37",
            "Answer 38",
            "Answer 39",
            "Answer 40",
            "Answer 41",
            "Answer 42",
            "Answer 43",
            "Answer 44",
            "Answer 45",
            "Answer 46",
            "Answer 47",
            "Answer 48",
            "Answer 49",
            "Answer 50",
            "Answer 51",
            "Answer 52",
            "Answer 53",
            "Answer 54",
            "Answer 55",
            "Answer 56",
            "Answer 57",
            "Answer 58",
            "Answer 59",
            "Answer 60",
            "Answer 61",
            "Answer 62",
            "Answer 63",
            "Answer 64",
            "Answer 65",
            "Answer 66",
            "Answer 67",
            "Answer 68",
            "Answer 69",
            "Answer 70",
            "Answer 71",
            "Answer 72",
            "Answer 73",
            "Answer 74",
            "Answer 75",
            "Answer 76",
            "Answer 77",
            "Answer 78",
            "Answer 79",
            "Answer 80",
            "Answer 81",
            "Answer 82",
            "Answer 83",
            "Answer 84",
            "Answer 85",
            "Answer 86",
            "Answer 87",
            "Answer 88",
            "Answer 89",
            "Answer 90",
            "Answer 91",
            "Answer 92",
            "Answer 93",
            "Answer 94",
            "Answer 95",
            "Answer 96",
            "Answer 97",
            "Answer 98",
            "Answer 99",
            "Answer 100"
        };
    }
}