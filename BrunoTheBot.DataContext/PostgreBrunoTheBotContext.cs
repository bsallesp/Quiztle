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
            modelBuilder.Entity<Author>();
            modelBuilder.Entity<Content>();
            modelBuilder.Entity<Place>();
            modelBuilder.Entity<School>();
            modelBuilder.Entity<TopicClass>();

            //QUIZ
            modelBuilder.Entity<Answer>();
            modelBuilder.Entity<Option>();
            modelBuilder.Entity<Question>();

            //AILOG
            modelBuilder.Entity<AILog>();
        }

        //COURSE
        public DbSet<Author>? Authors { get; set; }
        public DbSet<Content>? Contents { get; set; }
        public DbSet<Place>? Places { get; set; }
        public DbSet<School>? Schools { get; set; }
        public DbSet<TopicClass>? TopicClasses { get; set; }

        //QUIZ
        public DbSet<Answer>? Answers { get; set; }
        public DbSet<Option>? Options { get; set; }
        public DbSet<Question>? Questions { get; set; }

        //AILOG
        public DbSet<AILog>? AILogs { get; set; }
    }
}