using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BrunoTheBot.DataContext
{
    public class PostgreDataContextFactory : IDesignTimeDbContextFactory<PostgreBrunoTheBotContext>
    {
        public PostgreBrunoTheBotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreBrunoTheBotContext>();

            string connectionString = "Host=brunothebot-postgres;port=5432;Database=BrunoTheBotDB;Username=brunothebotuser;Password=@pyramid2050!";

            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreBrunoTheBotContext(optionsBuilder.Options);
        }
    }
}
