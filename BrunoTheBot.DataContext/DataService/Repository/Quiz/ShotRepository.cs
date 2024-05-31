using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;

namespace BrunoTheBot.DataContext.DataService.Repository.Quiz
{
    public class ShotRepository
    {
        private readonly PostgreBrunoTheBotContext _context;

        public ShotRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateShotAsync(Shot shot)
        {
            try
            {
                EnsureShotNotNull();
                _context.Shots!.Add(shot);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateShotAsync: An exception occurred while creating the shot:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Shot?> GetShotByIdAsync(Guid shotId)
        {
            return await _context.Shots!.FirstOrDefaultAsync(s => s.Id == shotId);
        }

        private void EnsureShotNotNull()
        {
            if (_context.Shots == null)
            {
                throw new InvalidOperationException("CreateShotAsync: The OptionsDTO DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}