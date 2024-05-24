using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;

namespace BrunoTheBot.DataContext.DataService.Repository.Quiz
{
    public class TestRepository
    {
        private readonly PostgreBrunoTheBotContext _context;
        public TestRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateTestAsync(Test test)
        {
            try
            {
                EnsureTestNotNull();
                _context.Tests!.Add(test);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateTestAsync: An exception occurred while creating the test:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<Test>> GetAllExamsAsync()
        {
            EnsureTestNotNull();
            var response = await _context.Tests!
                .Include(q => q.Questions)
                .ToListAsync();
            return response;
        }

        private void EnsureTestNotNull()
        {
            if (_context.Tests == null)
            {
                throw new InvalidOperationException("CreateTestAsync: The Tests DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
