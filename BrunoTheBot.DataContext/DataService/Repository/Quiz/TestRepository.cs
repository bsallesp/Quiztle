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

        public async Task AddQuestionsToTestAsync(Guid testId, List<Question> questions)
        {
            EnsureTestNotNull();
            var test = await _context.Tests!
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null)
            {
                throw new KeyNotFoundException($"No test found with ID {testId}");
            }

            foreach (var question in questions)
            {
                test.Questions.Add(question);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Test>> GetAllTestsByPDFDataIdAsync(Guid PDFDataId)
        {
            EnsureTestNotNull();
            var response = await _context.Tests!.Where(p => p.PDFDataId == PDFDataId)!
                .Include(q => q.Questions)
                .ToListAsync();
            return response;
        }

        public async Task<List<Test>> GetAllTestsAsync()
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
