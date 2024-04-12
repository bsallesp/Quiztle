using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;
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
            //COURSE
            modelBuilder.Entity<Book>();
            modelBuilder.Entity<Chapter>();
            modelBuilder.Entity<Section>();
            modelBuilder.Entity<Content>();

            //QUIZ
            modelBuilder.Entity<Question>();
            modelBuilder.Entity<Option>();

            //AILOG
            modelBuilder.Entity<AILog>();
        }

        //COURSE
        public DbSet<Content>? Contents { get; set; }
        public DbSet<Book>? Books { get; set; }
        public DbSet<Chapter>? Chapters { get; set; }
        public DbSet<Section>? Sections { get; set; }

        //QUIZ
        public DbSet<Option>? Options { get; set; }
        public DbSet<Question>? Questions { get; set; }

        //AILOG
        public DbSet<AILog>? AILogs { get; set; }
    }
}