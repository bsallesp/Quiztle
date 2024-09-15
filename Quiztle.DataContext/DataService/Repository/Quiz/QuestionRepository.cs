using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.DataContext.Repositories.Quiz
{
    public class QuestionRepository
    {
        private readonly PostgreQuiztleContext _context;

        public QuestionRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public QuestionRepository()
        {
        }

        public async Task<Question?> GetARandomQuestionToRate()
        {
            EnsureQuestionsNotNull();

            var question = await _context.Questions!
                .Where(q => !q.Verified)
                .OrderBy(q => Guid.NewGuid())
                .FirstOrDefaultAsync();

            return question;

        }

        public async Task CreateQuestionAsync(Question question)
        {
            try
            {
                EnsureQuestionsNotNull();
                _context.Questions!.Add(question);
                await _context.SaveChangesAsync();

                Console.WriteLine("QuestionRepository/CreateQuestionAsync success created: " + question.Id);
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

        public async Task<IEnumerable<Question?>> GetQuestionByDraftAsync(Guid id)
        {
            EnsureQuestionsNotNull();
            return await _context.Questions!.Where(q => q.Draft!.Id == id).ToListAsync();
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

        public async Task<bool> DeleteQuestionAsync(Guid id)
        {
            EnsureQuestionsNotNull();
            var question = await _context.Questions!.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        private void EnsureQuestionsNotNull()
        {
            if (_context.Questions == null)
            {
                throw new InvalidOperationException("The QuestionsDTO DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
