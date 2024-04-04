using BrunoTheBot.CoreBusiness.Log;
using Microsoft.EntityFrameworkCore;

namespace BrunoTheBot.DataContext.Repositories
{
    public class AILogRepository
    {
        private readonly PostgreBrunoTheBotContext _context;

        public AILogRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateAILogAsync(AILog aILog)
        {
            try
            {
                EnsureAILogNotNull();
                _context.AILogs!.Add(aILog);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the answer:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<AILog?> GetAILogByIdAsync(int id)
        {
            try
            {
                if (_context.AILogs == null)
                {
                    throw new InvalidOperationException("The AILogs DbSet is null. Make sure it is properly initialized.");
                }

                var result = await _context.AILogs.FirstOrDefaultAsync(x => x.Id == id);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while retrieving the AILog by ID:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void EnsureAILogNotNull()
        {
            if (_context.AILogs == null)
            {
                throw new InvalidOperationException("The AILog DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
