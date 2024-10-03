using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Prompts;

namespace Quiztle.DataContext.DataService.Repository
{
    public class PromptRepository
    {
        private readonly PostgreQuiztleContext _context;

        public PromptRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreatePromptAsync(Prompt prompt)
        {
            try
            {
                EnsurePromptNotNull();
                _context.Prompts!.Add(prompt);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreatePromptAsync: An exception occurred while creating the prompt:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Prompt?> GetPromptByIdAsync(Guid id)
        {
            try
            {
                EnsurePromptNotNull();
                return await _context.Prompts!
                    .Include(p => p.Items)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPromptByIdAsync: An exception occurred while retrieving the prompt by Id:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<Prompt?>> GetAllPromptsAsync()
        {
            try
            {
                EnsurePromptNotNull();
                return await _context.Prompts!
                    .Include(p => p.Items)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllPromptsAsync: An exception occurred while retrieving all prompts:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task DeletePromptAsync(Guid id)
        {
            try
            {
                EnsurePromptNotNull();
                var prompt = await _context.Prompts!.FindAsync(id);

                if (prompt == null)
                {
                    throw new InvalidOperationException("DeletePromptAsync: Prompt not found.");
                }

                _context.Prompts.Remove(prompt);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeletePromptAsync: An exception occurred while deleting the prompt:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void EnsurePromptNotNull()
        {
            if (_context.Prompts == null)
            {
                throw new InvalidOperationException("PromptRepository/EnsurePromptNotNull: The Prompt DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
