using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Log;
using BrunoTheBot.CoreBusiness.Entities.Tasks;
using BrunoTheBot.CoreBusiness.Entities.PDFData;

namespace BrunoTheBot.DataContext
{
    public class PostgreBrunoTheBotContext : DbContext
    {
        public PostgreBrunoTheBotContext(DbContextOptions<PostgreBrunoTheBotContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>();
            modelBuilder.Entity<Chapter>();
            modelBuilder.Entity<Section>();
            modelBuilder.Entity<Content>();
            modelBuilder.Entity<Question>();
            modelBuilder.Entity<Option>();
            modelBuilder.Entity<AILog>();
            modelBuilder.Entity<BookTask>();
            modelBuilder.Entity<PDFData>();
            modelBuilder.Entity<PDFDataPages>();
            modelBuilder.Entity<Test>();
            modelBuilder.Entity<Response>();
        }

        public DbSet<Content>? Contents { get; set; }
        public DbSet<Book>? Books { get; set; }
        public DbSet<Chapter>? Chapters { get; set; }
        public DbSet<Section>? Sections { get; set; }
        public DbSet<Option>? Options { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<AILog>? AILogs { get; set; }
        public DbSet<BookTask>? BookTasks { get; set; }
        public DbSet<PDFData>? PDFData { get; set; }
        public DbSet<PDFDataPages>? PDFDataPages { get; set; }
        public DbSet<Test>? Tests { get; set; }
        public DbSet<Response>? Responses { get; set; }
    }
}
