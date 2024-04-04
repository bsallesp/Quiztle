using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.DataContext;

namespace BrunoTheBot.DataContext
{
    public class SchoolSeed
    {
        private string RandomOne(string[] list)
        {
            var idx = new Random().Next(list.Length);
            return list[idx];
        }

        private School MakeSchool(List<Topic> topics)
        {
            var school = new School
            {
                Name = RandomOne(_names),
                Created = DateTime.Now,
                Topics = topics
            };
            return school;
        }

        public async Task SeedDatabaseWithSchoolCountAsync(PostgreBrunoTheBotContext context, int totalCount, List<Topic> topics)
        {
            var count = 0;
            var currentCycle = 0;
            while (count < totalCount)
            {
                var list = new List<School>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(MakeSchool(topics));
                }
                if (list.Count > 0)
                {
                    context.Schools?.AddRange(list);
                    await context.SaveChangesAsync();
                }
                currentCycle = 0;
            }
        }

        private readonly string[] _names = new[]
        {
            "School 1",
            "School 2",
            "School 3",
        };
    }
}
