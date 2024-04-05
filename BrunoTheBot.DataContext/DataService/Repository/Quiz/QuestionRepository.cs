using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities.Quiz;

namespace BrunoTheBot.DataContext.Repositories.Quiz
{
    public class QuestionRepository
    {
        private readonly PostgreBrunoTheBotContext _context;

        public QuestionRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateQuestionAsync(Question question)
        {
            try
            {
                EnsureQuestionsNotNull();
                _context.Questions!.Add(question);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the question:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            EnsureQuestionsNotNull();
            return await _context.Questions!.FindAsync(id);
        }

        public async Task<IQueryable<Question>> GetAllQuestionsAsync()
        {
            EnsureQuestionsNotNull();
            var questions = await _context.Questions!.ToListAsync();
            return questions.AsQueryable();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            EnsureQuestionsNotNull();
            _context.Entry(question).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            EnsureQuestionsNotNull();
            var question = await _context.Questions!.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureQuestionsNotNull()
        {
            if (_context.Questions == null)
            {
                throw new InvalidOperationException("The Questions DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
