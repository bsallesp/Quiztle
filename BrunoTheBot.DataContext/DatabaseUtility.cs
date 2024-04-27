//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace BrunoTheBot.DataContext
//{
//    public class DatabaseUtility
//    {
//        public static async Task EnsureDbCreatedAndSeedWithCountOfAsync(DbContextOptions<PostgreBrunoTheBotContext> options, int count)
//        {
//            var factory = new LoggerFactory();
//            var builder = new DbContextOptionsBuilder<PostgreBrunoTheBotContext>(options)
//                .UseLoggerFactory(factory);

//            using var context = new PostgreBrunoTheBotContext(builder.Options);
//            // Result is true if the database had to be created.

//            if (await context.Database.EnsureCreatedAsync())
//            {
//                var bookSeed = new BookSeed();
//                var optionSeed = new OptionSeed();
//                var questionSeed = new QuestionSeed();
//                var chapterSeed = new ChapterSeed();

//                await bookSeed.SeedDatabaseWithBookCountAsync(context, count, chapterSeed.GetChapterAsync(50).Result);
//                await chapterSeed.SeedDatabaseWithChapterCountAsync(context, count);
//                await questionSeed.SeedDatabaseWithQuestionCountAsync(context, count);
//                await optionSeed.SeedDatabaseWithOptionsCountAsync(context, count);
//            }
//        }
//    }
//}
