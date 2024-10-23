using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Performance;

namespace Quiztle.DataContext.DataService.Repository.Performance
{
    public class TestPerformanceRepository
    {
        private readonly PostgreQuiztleContext _context;

        public TestPerformanceRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateTestPerformanceAsync(TestPerformance testPerformance)
        {
            try
            {
                EnsureTestPerformanceNotNull();
                _context.TestsPerformance!.Add(testPerformance);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateTestPerformanceAsync: An exception occurred while creating the test performance:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<TestPerformance?>> GetTestPerformancesByUserAsync(Guid userId)
        {
            try
            {
                EnsureTestPerformanceNotNull();
                return await _context.TestsPerformance!.Where(u => u.UserId == userId)
                    .Include(tp => tp.QuestionsPerformance)
                    .AsTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetTestPerformancesByUserAsync:" +
                    " An exception occurred while retrieving the test performance by User:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<TestPerformance?> GetTestPerformanceByIdAsync(Guid id)
        {
            try
            {
                EnsureTestPerformanceNotNull();
                return await _context.TestsPerformance!
                    .Include(tp => tp.QuestionsPerformance) // Ajuste conforme necessário
                    .AsTracking()
                    .FirstOrDefaultAsync(tp => tp.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetTestPerformanceByIdAsync: An exception occurred while retrieving the test performance by Id:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<TestPerformance>> GetAllTestPerformancesAsync()
        {
            try
            {
                EnsureTestPerformanceNotNull();
                return await _context.TestsPerformance!
                    .Include(tp => tp.QuestionsPerformance) // Ajuste conforme necessário
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllTestPerformancesAsync: An exception occurred while retrieving all test performances:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task UpdateTestPerformanceAsync(TestPerformance testPerformance)
        {
            try
            {
                EnsureTestPerformanceNotNull();

                var existingTestPerformance = await _context.TestsPerformance!
                    .FirstOrDefaultAsync(tp => tp.Id == testPerformance.Id);

                if (existingTestPerformance == null)
                {
                    throw new InvalidOperationException("UpdateTestPerformanceAsync: The TestPerformance to update does not exist.");
                }

                _context.Entry(existingTestPerformance).CurrentValues.SetValues(testPerformance);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateTestPerformanceAsync: An exception occurred while updating the test performance:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void EnsureTestPerformanceNotNull()
        {
            if (_context.TestsPerformance == null)
            {
                throw new InvalidOperationException("TestPerformanceRepository/EnsureTestPerformanceNotNull: The TestPerformance DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
