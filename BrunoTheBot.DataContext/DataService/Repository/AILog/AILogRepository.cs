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
                throw new Exception("An exception occurred while creating the answer.", ex);
            }
        }

        public async Task<AILog?> GetAILogByIdAsync(Guid id)
        {
            try
            {
                if (_context.AILogs == null)
                {
                    throw new InvalidOperationException("The AILogs DbSet is null. Make sure it is properly initialized.");
                }

                return await _context.AILogs.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An exception occurred: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }
                errorMessage += $" StackTrace: {ex.StackTrace}";
                throw new Exception(errorMessage);
            }
        }

        public async Task<List<AILog>> GetAllAILogsAsync()
        {
            try
            {
                EnsureAILogNotNull();
                return await _context.AILogs!.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occurred while retrieving all AILogs.", ex);
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
