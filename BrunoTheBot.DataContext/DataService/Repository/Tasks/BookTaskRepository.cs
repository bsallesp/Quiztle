using BrunoTheBot.CoreBusiness.Entities.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BrunoTheBot.DataContext.DataService.Repository.Tasks
{
    public class BookTaskRepository
    {
        private readonly PostgreBrunoTheBotContext _context;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public BookTaskRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<BookTask>> GetBookTasks(byte status = 0)
        {
            try
            {
                EnsureBooksNotNull();
                var bookTaskList = await _context.BookTasks!.Where(bt => bt.Status == 0).ToListAsync();
                return bookTaskList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the BookTask:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<BookTask>> GetAllBookTasks()
        {
            try
            {
                EnsureBooksNotNull();
                var bookTaskList = await _context.BookTasks!.ToListAsync();
                return bookTaskList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the BookTask:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<string> GetNextBookTaskFromQueue()
        {
            try
            {
                await _semaphore.WaitAsync();

                EnsureBooksNotNull();

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var bookTask = await _context.BookTasks!
                        .Where(bt => bt.Status == 0)
                        .FirstOrDefaultAsync();

                    if (bookTask != null)
                    {
                        _context.BookTasks!.Remove(bookTask);

                        await _context.SaveChangesAsync();

                        // Commit the transaction
                        await transaction.CommitAsync();

                        return bookTask.Name!;
                    }
                    else
                    {
                        Console.WriteLine("NOT Found.");
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the BookTask:");
                Console.WriteLine(ex.ToString());

                return "";
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> CreateBookTaskAsync(BookTask bookTask)
        {
            try
            {
                EnsureBooksNotNull();
                bookTask.Id = Guid.NewGuid();
                _context.BookTasks!.Add(bookTask);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the BookTask:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void EnsureBooksNotNull()
        {
            if (_context.Books == null)
            {
                throw new InvalidOperationException("The Books DbSet is null. Make sure it is properly initialized.");
            }
        }

        public async Task<bool> DeleteAll()
        {
            try
            {
                EnsureBooksNotNull();

                var allBookTasks = await _context.BookTasks!.ToListAsync();
                _context.BookTasks!.RemoveRange(allBookTasks);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while deleting all BookTasks:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
