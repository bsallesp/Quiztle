using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.DataContext.DataService.Repository.Course
{
    public class BookRepository
    {
        private readonly PostgreBrunoTheBotContext _context;

        public BookRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateBookAsync(Book book)
        {
            try
            {
                EnsureBooksNotNull();
                _context.Books!.Add(book);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the book:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(Guid id, bool showChapters = false, bool showSections = false, bool showQuestions = false)
        {
            EnsureBooksNotNull();

            var query = _context.Books!.AsQueryable();

            query = query.Include(s => s.Chapters);

            if (showChapters)
            {
                if (showSections)
                {
                    query = query.Include(s => s.Chapters)
                                 .ThenInclude(t => t.Sections);
                }
                else
                {
                    query = query.Include(s => s.Chapters);
                }
            }

            if (showSections)
            {
                query = query.Include(s => s.Chapters)
                             .ThenInclude(t => t.Sections)
                             .ThenInclude(c => c.Content);
            }

            if (showQuestions)
            {
                query = query.Include(s => s.Chapters)
                             .ThenInclude(t => t.Sections)
                             .ThenInclude(q => q.Questions)
                             .ThenInclude(o => o.Options);
            }

            Book book = await query.FirstOrDefaultAsync(s => s.Id == id) ?? new Book();

            return book;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            EnsureBooksNotNull();
            var books = await _context.Books!.ToListAsync();
            return [.. books.AsQueryable()];
        }

        public async Task UpdateBookAsync(Book book)
        {
            EnsureBooksNotNull();
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            EnsureBooksNotNull();
            var book = await _context.Books!.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureBooksNotNull()
        {
            if (_context.Books == null)
            {
                throw new InvalidOperationException("The Books DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
