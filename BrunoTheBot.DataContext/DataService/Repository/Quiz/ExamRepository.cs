using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;

namespace BrunoTheBot.DataContext.DataService.Repository.Quiz
{
    public class ExamRepository
    {
        private readonly PostgreBrunoTheBotContext _context;
        public ExamRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateExamAsync(Exam exam)
        {
            try
            {
                EnsureExamNotNull();
                _context.Exams!.Add(exam);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateExamAsync: An exception occurred while creating the exam:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<Exam>> GetAllExamsAsync()
        {
            EnsureExamNotNull();
            var response = await _context.Exams!
                .Include(q => q.Questions)
                .ToListAsync();
            return response;
        }


        private void EnsureExamNotNull()
        {
            if (_context.Exams == null)
            {
                throw new InvalidOperationException("CreateExamAsync: The Exams DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
