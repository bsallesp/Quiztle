using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BrunoTheBot.DataContext
{
    public class PostgreDataContextFactory : IDesignTimeDbContextFactory<PostgreBrunoTheBotContext>
    {
        public PostgreBrunoTheBotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreBrunoTheBotContext>();

            string connectionString = ConnectionStrings.DevelopmentConnectionString;

            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreBrunoTheBotContext(optionsBuilder.Options);
        }
    }
}
