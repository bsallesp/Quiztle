using Quiztle.CoreBusiness.Entities.Course;
using Quiztle.DataContext;

namespace Quiztle.DataContext
{
    public class BookSeed
    {
        private string RandomOne(string[] list)
        {
            var idx = new Random().Next(list.Length);
            return list[idx];
        }

        private Book MakeBook(List<Chapter> chapters)
        {
            var book = new Book
            {
                Name = RandomOne(_names),
                Created = DateTime.Now,
                Chapters = chapters
            };
            return book;
        }

        public async Task SeedDatabaseWithBookCountAsync(PostgreQuiztleContext context, int totalCount, List<Chapter> chapters)
        {
            var count = 0;
            var currentCycle = 0;
            while (count < totalCount)
            {
                var list = new List<Book>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(MakeBook(chapters));
                }
                if (list.Count > 0)
                {
                    context.Books?.AddRange(list);
                    await context.SaveChangesAsync();
                }
                currentCycle = 0;
            }
        }

        private readonly string[] _names = new[]
        {
            "Book 1",
            "Book 2",
            "Book 3",
        };
    }
}
