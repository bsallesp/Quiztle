using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Quiz;
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

        public async Task CreateScratchAsync(Scratch scratch)
        {
            try
            {
                EnsureScratchNotNull();
                _context.Scratches!.Add(scratch);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateScratchAsync: An exception occurred while creating the test:");
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
                    .Include(s => s.Drafts!)
                    .ThenInclude(d => d.Questions)
                    .AsTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetScratchByIdAsync: An exception occurred while retrieving the scratch by Id:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task SaveChangesAsync(Test test)
        {
            await _context!.Tests!.AddAsync(test);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Scratch?>> GetAllScratchesAsync()
        {
            try
            {
                EnsureScratchNotNull();
                return await _context.Scratches!
                    .Include(s => s.Drafts!) 
                        .ThenInclude(d => d.Questions!)
                            .ThenInclude(q => q.Options)
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

        public async Task<IEnumerable<Scratch?>> GetFilteredScratchesAsync()
        {
            try
            {
                EnsureScratchNotNull();

                var scratches = await _context.Scratches!
                    .Include(s => s.Drafts!)
                        .ThenInclude(d => d.Questions!)
                            .ThenInclude(q => q.Options)
                    .AsNoTracking()
                    .ToListAsync();

                var filteredScratches = scratches
                    .Select(scratch => new Scratch
                    {
                        // Copia as propriedades do Scratch
                        Id = scratch.Id,
                        Name = scratch.Name,
                        Drafts = scratch.Drafts!
                            .Select(draft => new Draft
                            {
                                // Copia as propriedades do Draft
                                Id = draft.Id,
                                Text = draft.Text,
                                Questions = draft.Questions!
                                    .Where(q => q.Options.Any() &&
                                                !string.IsNullOrEmpty(q.Hint) &&
                                                !string.IsNullOrEmpty(q.Resolution))
                                    .ToList()
                            })
                            .Where(draft => draft.Questions!.Count != 0)
                            .ToList()
                    })
                    .Where(scratch => scratch.Drafts!.Count != 0)
                    .ToList();

                return filteredScratches;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetFilteredScratchesAsync: An exception occurred while filtering scratches:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task UpdateScratchAsync(Scratch scratch)
        {
            try
            {
                EnsureScratchNotNull();

                // Verifica se o Scratch existe
                var existingScratch = await _context.Scratches!
                    .Include(s => s.Drafts) // Incluindo os Drafts relacionados se necessário
                    .FirstOrDefaultAsync(s => s.Id == scratch.Id);

                if (existingScratch == null)
                {
                    throw new InvalidOperationException("UpdateScratchAsync: The Scratch to update does not exist.");
                }

                // Atualiza as propriedades do Scratch existente com os valores do scratch fornecido
                _context.Entry(existingScratch).CurrentValues.SetValues(scratch);

                // Se você precisar atualizar os Drafts também, você pode fazer isso aqui
                // Certifique-se de manipular os Drafts com cuidado para evitar problemas de concorrência e duplicação

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateScratchAsync: An exception occurred while updating the scratch:");
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
