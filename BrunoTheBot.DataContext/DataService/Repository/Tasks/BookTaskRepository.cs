using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.Entities.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

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
                throw;
            }
        }

        public async Task<APIResponse<BookTask>> GetNextBookTaskFromQueue()
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

                    if (bookTask == null) return new APIResponse<BookTask>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = bookTask ?? new BookTask(),
                        Message = "No tasks to execute."
                    };

                    if (bookTask.Name == "" || bookTask == null) return new APIResponse<BookTask>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new BookTask(),
                        Message = "Book Task Name is empty or null: "
                    };

                    return new APIResponse<BookTask>
                    {
                        Status = CustomStatusCodes.SuccessStatus,
                        Data = bookTask,
                        Message = "New task caught: "
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<BookTask>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new BookTask(),
                    Message = ex.Data.ToString()
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<APIResponse<BookTask>> FinishBookTask(Guid Id)
        {
            try
            {
                EnsureBooksNotNull();

                var bookTask = await _context.BookTasks!
                    .Where(bt => bt.Id == Id)
                    .FirstOrDefaultAsync();

                if (bookTask == null) return new APIResponse<BookTask>
                {
                    Status = CustomStatusCodes.NotFound,
                    Data = bookTask ?? new BookTask(),
                    Message = "No tasks found with Guid: " + Id
                };

                bookTask.Status = 2;
                await _context.SaveChangesAsync();

                return new APIResponse<BookTask>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = bookTask,
                    Message = "Book Task set to finished."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<BookTask>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new BookTask(),
                    Message = ex.Data.ToString()
                };
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
