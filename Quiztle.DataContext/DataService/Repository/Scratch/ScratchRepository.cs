using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Scratch;

namespace Quiztle.DataContext.DataService.Repository
{
    public class ScratchRepository
    {
        private readonly PostgreQuiztleContext _context;
        public ScratchRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateTestAsync(Scratch scratch)
        {
            try
            {
                EnsureScratchNotNull();
                _context.Scratches!.Add(scratch);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateTestAsync: An exception occurred while creating the test:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Scratch?> GetScratchByIdAsync(Guid id)
        {
            try
            {
                EnsureScratchNotNull();
                return await _context.Scratches!
                    .Include(s => s.Drafts) // Incluindo os Drafts relacionados
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetScratchByIdAsync: An exception occurred while retrieving the scratch by Id:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        public async Task<IEnumerable<Scratch?>> GetAllScratchesAsync()
        {
            try
            {
                EnsureScratchNotNull();
                return await _context.Scratches!
                    .Include(s => s.Drafts) // Incluindo os Drafts relacionados
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllScratchesAsync: An exception occurred while retrieving all scratches:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        private void EnsureScratchNotNull()
        {
            if (_context.Scratches == null)
            {
                throw new InvalidOperationException("ScratchRepository/EnsureScratchNotNull: The Scratch DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
