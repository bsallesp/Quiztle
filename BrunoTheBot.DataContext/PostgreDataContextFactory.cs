//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;

//namespace BrunoTheBot.DataContext
//{
//    public class PostgreDataContextFactory : IDesignTimeDbContextFactory<PostgreDataContextFactory>
//    {
//        public IConfiguration? Configuration { get; }

//        public PostgreDataContextFactory CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<PostgreBrunoTheBotContext>();

//            string connectionString = ConnectionStrings.DevelopmentConnectionString;

//            Console.WriteLine("Opening " + connectionString);

//            optionsBuilder.UseNpgsql(connectionString);

//            return new PostgreBrunoTheBotContext();
//        }
//    }
//}
