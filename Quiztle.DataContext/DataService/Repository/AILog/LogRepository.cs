using Quiztle.CoreBusiness.Log;
using Microsoft.EntityFrameworkCore;

namespace Quiztle.DataContext.Repositories
{
    public class LogRepository
    {
        private readonly PostgreQuiztleContext _context;

        public LogRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateLogAsync(Log log)
        {
            try
            {
                EnsureLogNotNull();
                _context.Logs!.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occurred while creating the log.", ex);
            }
        }

        private void EnsureLogNotNull()
        {
            if (_context.Logs == null)
            {
                throw new InvalidOperationException("The Log DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
