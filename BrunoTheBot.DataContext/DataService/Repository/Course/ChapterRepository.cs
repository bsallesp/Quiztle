using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.DataContext.DataService.Repository.Course
{
    public class ChapterRepository
    {
        private readonly PostgreBrunoTheBotContext _context;

        public ChapterRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateChapterAsync(Chapter chapter)
        {
            try
            {
                EnsureChaptersIsNotNull();
                _context.Chapters!.Add(chapter);
                await _context.SaveChangesAsync();
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

        public async Task<Chapter?> GetChapterByIdAsync(int id)
        {
            EnsureChaptersIsNotNull();
            return await _context.Chapters!.FindAsync(id);
        }

        public async Task<IQueryable<Chapter>> GetAllChaptersAsync()
        {
            EnsureChaptersIsNotNull();
            var chapter = await _context.Chapters!.ToListAsync();
            return chapter.AsQueryable();
        }

        public async Task UpdateChapterAsync(Chapter chapter)
        {
            EnsureChaptersIsNotNull();
            _context.Entry(chapter).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChapterrAsync(int id)
        {
            EnsureChaptersIsNotNull();
            var chapter = await _context.Chapters!.FindAsync(id);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureChaptersIsNotNull()
        {
            if (_context.Chapters == null)
            {
                throw new InvalidOperationException("The Chapter DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
