using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities.Quiz;

namespace BrunoTheBot.DataContext.Repositories.Quiz
{
    public class OptionRepository
    {
        private readonly PostgreBrunoTheBotContext _context;

        public OptionRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateOptionAsync(Option option)
        {
            try
            {
                EnsureOptionsNotNull();
                _context.Options!.Add(option);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the option:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Option?> GetOptionByIdAsync(int id)
        {
            EnsureOptionsNotNull();
            return await _context.Options!.FindAsync(id);
        }

        public async Task<IQueryable<Option>> GetAllOptionsAsync()
        {
            EnsureOptionsNotNull();
            var response = await _context.Options!.ToListAsync();
            return response.AsQueryable();
        }

        public async Task UpdateOptionAsync(Option option)
        {
            EnsureOptionsNotNull();
            _context.Entry(option).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOptionAsync(int id)
        {
            EnsureOptionsNotNull();
            var option = await _context.Options!.FindAsync(id);
            if (option != null)
            {
                _context.Options.Remove(option);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureOptionsNotNull()
        {
            if (_context.Options == null)
            {
                throw new InvalidOperationException("The Options DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
