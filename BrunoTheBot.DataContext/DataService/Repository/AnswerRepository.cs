using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness;

namespace BrunoTheBot.DataContext
{
    public class AnswerRepository
    {
        private readonly SqliteDataContext _context;

        public AnswerRepository(SqliteDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateAnswerAsync(Answer answer)
        {
            try
            {
                EnsureAnswersNotNull();
                _context.Answers!.Add(answer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the answer:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Answer?> GetAnswerByIdAsync(int id)
        {
            EnsureAnswersNotNull();
            return await _context.Answers.FindAsync(id);
        }

        public async Task<IQueryable<Answer>> GetAllAnswersAsync()
        {
            EnsureAnswersNotNull();
            return _context.Answers!.AsQueryable();
        }

        // Update
        public async Task UpdateAnswerAsync(Answer answer)
        {
            EnsureAnswersNotNull();
            _context.Entry(answer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteAnswerAsync(int id)
        {
            EnsureAnswersNotNull();
            var answer = await _context.Answers!.FindAsync(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureAnswersNotNull()
        {
            if (_context.Answers == null)
            {
                throw new InvalidOperationException("The Answers DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
