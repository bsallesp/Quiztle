using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BrunoTheBot.DataContext
{
    public class SqliteDataContextFactory : IDesignTimeDbContextFactory<SqliteDataContext>
    {
        public IConfiguration? Configuration { get; }

        public SqliteDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqliteDataContext>();

            string databaseName = "BrunoTheBotDb.db";
            string directoryName = "BrunoTheBotDb";

            string appPath = ConnectionStrings.DevelopmentConnectionString;

            Console.WriteLine("Opening " + appPath);

            string dbPath = Path.Combine(appPath, directoryName, databaseName);

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new SqliteDataContext(optionsBuilder.Options);
        }
    }
}
