using Quiztle.CoreBusiness.Entities.Course;
using Quiztle.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Log;
using Quiztle.CoreBusiness.Entities.Tasks;
using Quiztle.CoreBusiness.Entities.PDFData;

namespace Quiztle.DataContext
{
    public class PostgreQuiztleContext : DbContext
    {
        public PostgreQuiztleContext(DbContextOptions<PostgreQuiztleContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>();
            modelBuilder.Entity<Chapter>();
            modelBuilder.Entity<Section>();
            modelBuilder.Entity<Content>();
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId);

            modelBuilder.Entity<Option>();
            modelBuilder.Entity<AILog>();
            modelBuilder.Entity<BookTask>();
            modelBuilder.Entity<PDFData>();
            modelBuilder.Entity<PDFDataPages>();
            modelBuilder.Entity<Test>();
            modelBuilder.Entity<Response>()
                .HasOne<Test>()
                .WithMany(t => t.Responses)
                .HasForeignKey(r => r.TestId);

            modelBuilder.Entity<Shot>();
            
            base.OnModelCreating(modelBuilder);
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
        public DbSet<Shot>? Shots { get; set; }
    }
}
