using BrunoTheBot.CoreBusiness;

namespace BrunoTheBot.DataContext
{
    public class AILogRepository
    {
        private readonly SqliteDataContext _context;

        public AILogRepository(SqliteDataContext context)
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

        private void EnsureAILogNotNull()
        {
            if (_context.AILogs == null)
            {
                throw new InvalidOperationException("The AILog DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
