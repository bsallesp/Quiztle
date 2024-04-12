using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.DataContext
{
    public class ChapterSeed
    {
        private string RandomOne(string[] list)
        {
            var idx = new Random().Next(list.Length);
            return list[idx];
        }

        private Chapter MakeChapter()
        {
            var chapter = new Chapter
            {
                Name = RandomOne(_names),
                Created = DateTime.Now
            };
            return chapter;
        }

        public async Task SeedDatabaseWithChapterCountAsync(PostgreBrunoTheBotContext context, int totalCount)
        {
            var count = 0;
            var currentCycle = 0;
            while (count < totalCount)
            {
                var list = new List<Chapter>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(MakeChapter());
                }
                if (list.Count > 0)
                {
                    context.Chapters?.AddRange(list);
                    await context.SaveChangesAsync();
                }
                currentCycle = 0;
            }
        }

        public Task<List<Chapter>> GetChapterAsync(int totalCount)
        {
            var count = 0;
            var currentCycle = 0;
            var chapters = new List<Chapter>();

            while (count < totalCount)
            {
                var list = new List<Chapter>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(MakeChapter());
                }
                if (list.Count > 0)
                {
                    chapters.AddRange(list);
                }
                currentCycle = 0;
            }

            return Task.FromResult(chapters);
        }

        private readonly string[] _names = new[]
        {
            "Chapter 1",
            "Chapter 2",
            "Chapter 3",
            "Chapter 4",
            "Chapter 5",
            "Chapter 6",
            "Chapter 7",
            "Chapter 8",
            "Chapter 9",
            "Chapter 10",
            "Chapter 11",
            "Chapter 12",
            "Chapter 13",
            "Chapter 14",
            "Chapter 15",
            "Chapter 16",
            "Chapter 17",
            "Chapter 18",
            "Chapter 19",
            "Chapter 20",
            "Chapter 21",
            "Chapter 22",
            "Chapter 23",
            "Chapter 24",
            "Chapter 25",
            "Chapter 26",
            "Chapter 27",
            "Chapter 28",
            "Chapter 29",
            "Chapter 30",
            "Chapter 31",
            "Chapter 32",
            "Chapter 33",
            "Chapter 34",
            "Chapter 35",
            "Chapter 36",
            "Chapter 37",
            "Chapter 38",
            "Chapter 39",
            "Chapter 40",
            "Chapter 41",
            "Chapter 42",
            "Chapter 43",
            "Chapter 44",
            "Chapter 45",
            "Chapter 46",
            "Chapter 47",
            "Chapter 48",
            "Chapter 49",
            "Chapter 50",
            "Chapter 51",
            "Chapter 52",
            "Chapter 53",
            "Chapter 54",
            "Chapter 55",
            "Chapter 56",
            "Chapter 57",
            "Chapter 58",
            "Chapter 59",
            "Chapter 60",
            "Chapter 61",
            "Chapter 62",
            "Chapter 63",
            "Chapter 64",
            "Chapter 65",
            "Chapter 66",
            "Chapter 67",
            "Chapter 68",
            "Chapter 69",
            "Chapter 70",
            "Chapter 71",
            "Chapter 72",
            "Chapter 73",
            "Chapter 74",
            "Chapter 75",
            "Chapter 76",
            "Chapter 77",
            "Chapter 78",
            "Chapter 79",
            "Chapter 80",
            "Chapter 81",
            "Chapter 82",
            "Chapter 83",
            "Chapter 84",
            "Chapter 85",
            "Chapter 86",
            "Chapter 87",
            "Chapter 88",
            "Chapter 89",
            "Chapter 90",
            "Chapter 91",
            "Chapter 92",
            "Chapter 93",
            "Chapter 94",
            "Chapter 95",
            "Chapter 96",
            "Chapter 97",
            "Chapter 98",
            "Chapter 99",
            "Chapter 100"
        };
    }
}
