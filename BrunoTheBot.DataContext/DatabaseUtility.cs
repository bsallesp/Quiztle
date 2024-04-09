﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BrunoTheBot.DataContext
{
    public class DatabaseUtility
    {
        public static async Task EnsureDbCreatedAndSeedWithCountOfAsync(DbContextOptions<PostgreBrunoTheBotContext> options, int count)
        {
            var factory = new LoggerFactory();
            var builder = new DbContextOptionsBuilder<PostgreBrunoTheBotContext>(options)
                .UseLoggerFactory(factory);

            using var context = new PostgreBrunoTheBotContext(builder.Options);
            // Result is true if the database had to be created.

            if (await context.Database.EnsureCreatedAsync())
            {
                var schoolSeed = new SchoolSeed();
                var optionSeed = new OptionSeed();
                var questionSeed = new QuestionSeed();
                var topicSeed = new TopicSeed();

                await schoolSeed.SeedDatabaseWithSchoolCountAsync(context, count, topicSeed.GetTopicsAsync(50).Result);
                await topicSeed.SeedDatabaseWithTopicCountAsync(context, count);
                await questionSeed.SeedDatabaseWithQuestionCountAsync(context, count);
                await optionSeed.SeedDatabaseWithOptionsCountAsync(context, count);
            }
        }
    }
}
