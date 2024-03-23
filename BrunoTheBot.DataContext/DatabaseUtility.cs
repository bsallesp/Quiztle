using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BrunoTheBot.CoreBusiness;

namespace BrunoTheBot.DataContext
{
    public class DatabaseUtility
    {
        public static async Task EnsureDbCreatedAndSeedWithCountOfAsync(DbContextOptions<SqliteDataContext> options, int count)
        {
            var factory = new LoggerFactory();
            var builder = new DbContextOptionsBuilder<SqliteDataContext>(options)
                .UseLoggerFactory(factory);

            using var context = new SqliteDataContext(builder.Options);
            // Result is true if the database had to be created.

            if (await context.Database.EnsureCreatedAsync())
            {
                var schoolSeed = new SchoolSeed();
                var answerSeed = new AnswerSeed();
                var optionSeed = new OptionSeed();
                var questionSeed = new QuestionSeed();
                var topicSeed = new TopicSeed();

                await schoolSeed.SeedDatabaseWithSchoolCountAsync(context, count, topicSeed.GetTopicsAsync(50).Result);
                await topicSeed.SeedDatabaseWithTopicCountAsync(context, count);
                await questionSeed.SeedDatabaseWithQuestionCountAsync(context, count);
                await optionSeed.SeedDatabaseWithOptionsCountAsync(context, count);
                await answerSeed.SeedDatabaseWithAnswerCountAsync(context, count);
            }
        }
    }
}
