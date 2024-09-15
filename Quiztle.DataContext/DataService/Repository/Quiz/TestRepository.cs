using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;

namespace Quiztle.DataContext.DataService.Repository.Quiz
{
    public class TestRepository
    {
        private readonly PostgreQuiztleContext _context;

        public TestRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> RemoveTestById(Guid id)
        {
            try
            {
                EnsureTestNotNull();
                var test = await _context.Tests!.FindAsync(id);
                if (test == null)
                {
                    // Caso o teste não seja encontrado
                    throw new KeyNotFoundException("Test not found");
                }

                _context.Tests.Remove(test);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (KeyNotFoundException ex)
            {
                // Lida com o caso onde o teste não é encontrado
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Lida com qualquer outra exceção
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
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

        public async Task<Test?> GetTestByIdAsync(Guid testId)
        {
            return await _context.Tests!
                .Include(t => t.Questions)
                .ThenInclude(o => o.Options)
                .FirstOrDefaultAsync(t => t.Id == testId);
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

        public async Task UpdateTest(Test test)
        {
            EnsureTestNotNull();
            var existingTest = await _context.Tests!.FirstOrDefaultAsync(t => t.Id == test.Id);
            if (existingTest == null)
                throw new Exception("Test with ID: " + test.Id + " not found for update.");
            else
            {
                existingTest.Name = test.Name;
                existingTest.Responses = test.Responses;
                existingTest.Questions = test.Questions;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Test?> GetTestByNameAsync(string name)
        {
            EnsureTestNotNull();
            return await _context.Tests!
                .Include(t => t.Questions)
                .ThenInclude(o => o.Options)
                .FirstOrDefaultAsync(t => t.Name == name);
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
