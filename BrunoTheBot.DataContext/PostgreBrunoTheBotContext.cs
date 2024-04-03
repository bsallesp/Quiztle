using BrunoTheBot.CoreBusiness.Entities;
using BrunoTheBot.CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace BrunoTheBot.DataContext
{
    public class PostgreBrunoTheBotContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(ConnectionStrings.DevelopmentConnectionString);

        public DbSet<School>? Schools { get; set; }
        public DbSet<Topic>? Topics { get; set; }
        public DbSet<Author>? Authors { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<Option>? Options { get; set; }
        public DbSet<Answer>? Answers { get; set; }
        public DbSet<AILog>? AILogs { get; set; }
    }
}