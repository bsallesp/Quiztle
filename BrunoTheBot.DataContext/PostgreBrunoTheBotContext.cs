using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.Log;

namespace BrunoTheBot.DataContext
{
    public class PostgreBrunoTheBotContext : DbContext
    {
        public PostgreBrunoTheBotContext(DbContextOptions<PostgreBrunoTheBotContext> options) : base(options)
        {            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseNpgsql(ConnectionStrings.DevelopmentConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // QUIZ GROUP
            modelBuilder.Entity<Answer>();
            modelBuilder.Entity<Option>();
            modelBuilder.Entity<Question>();
            modelBuilder.Entity<School>();
            modelBuilder.Entity<Topic>();
            modelBuilder.Entity<Reference>();

            // COURSE GROUP
            modelBuilder.Entity<Content>();
            modelBuilder.Entity<Author>();

            // LOG GROUP
            modelBuilder.Entity<AILog>();
        }


        // QUIZ GROUP
        public DbSet<Answer>? Answers { get; set; }
        public DbSet<Option>? Options { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<School>? Schools { get; set; }
        public DbSet<Topic>? Topics { get; set; }
        public DbSet<Reference>? References { get; set; }

        // COURSE GROUP
        public DbSet<Content>? Contents { get; set; }
        public DbSet<Author>? Authors { get; set; }

        // LOG GROUP
        public DbSet<AILog>? AILogs { get; set; }
    }
}