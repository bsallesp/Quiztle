using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Quiztle.DataContext
{
    public class PostgreDataContextFactory : IDesignTimeDbContextFactory<PostgreQuiztleContext>
    {
        public PostgreQuiztleContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreQuiztleContext>();

            string connectionString = "Host=Quiztle-postgres;Database=QuiztleDB;Username=Quiztleuser;Password=@pyramid2050!";

            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreQuiztleContext(optionsBuilder.Options);
        }
    }
}
