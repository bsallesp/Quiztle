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
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
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
        }

        public DbSet<Content>? Contents { get; set; }
        public DbSet<Book>? Books { get; set; }
        public DbSet<Chapter>? Chapters { get; set; }
        public DbSet<Section>? Sections { get; set; }
        public DbSet<Option>? Options { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<AILog>? AILogs { get; set; }
    }
}