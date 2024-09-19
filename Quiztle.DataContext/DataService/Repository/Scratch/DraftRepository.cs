using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Scratch;

namespace Quiztle.DataContext.DataService.Repository
{
    public class DraftRepository
    {
        private readonly PostgreQuiztleContext _context;

        public DraftRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateDraftAsync(Draft draft)
        {
            try
            {
                EnsureDraftNotNull();
                _context.Drafts!.Add(draft);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateDraftAsync: An exception occurred while creating the draft:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Draft?> GetDraftByIdAsync(Guid id)
        {
            try
            {
                EnsureDraftNotNull();
                return await _context.Drafts!
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDraftByIdAsync: An exception occurred while retrieving the draft by Id:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<Draft?>> GetAllDraftsAsync()
        {
            try
            {
                EnsureDraftNotNull();
                return await _context.Drafts!
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllDraftsAsync: An exception occurred while retrieving all drafts:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task UpdateDraftAsync(Draft draft)
        {
            try
            {
                EnsureDraftNotNull();

                // Verifica se o Draft existe
                var existingDraft = await _context.Drafts!
                    .Include(d => d.Questions) // Incluir perguntas associadas
                    .FirstOrDefaultAsync(d => d.Id == draft.Id);

                if (existingDraft == null)
                {
                    throw new InvalidOperationException("UpdateDraftAsync: The Draft to update does not exist.");
                }

                // Atualiza as propriedades do Draft existente com os valores do draft fornecido
                _context.Entry(existingDraft).CurrentValues.SetValues(draft);

                // Atualiza as perguntas associadas
                if (draft.Questions != null)
                {
                    // Remove perguntas antigas se necessário
                    existingDraft.Questions!.Clear();
                    foreach (var question in draft.Questions)
                    {
                        existingDraft.Questions.Add(question);
                    }
                }

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateDraftAsync: An exception occurred while updating the draft:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Draft?> GetDraftWithQuestionsAsync(Guid id, bool trackChanges = false)
        {
            try
            {
                EnsureDraftNotNull();

                // Define se o rastreamento de alterações será feito ou não
                var query = trackChanges
                    ? _context.Drafts!.Include(d => d.Questions).AsTracking()
                    : _context.Drafts!.Include(d => d.Questions).AsNoTracking();

                // Busca o draft com as perguntas
                return await query.FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDraftWithQuestionsAsync: An exception occurred while retrieving the draft with questions:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }



        public async Task<Draft?> GetNextDraftToMakeQuestionsAsync()
        {
            try
            {
                EnsureDraftNotNull();

                // Check if _context.Drafts is not null
                if (_context.Drafts == null)
                {
                    throw new InvalidOperationException("Drafts collection is null.");
                }

                // Fetch drafts and filter in memory
                var drafts = await _context.Drafts
                    .AsNoTracking()
                    .Include(q => q.Questions)
                    .ToListAsync();

                Draft? draftWithGreatestDifference = null;
                int maxDifference = int.MinValue;

                foreach (var draft in drafts)
                {
                    var questionCount = draft.Questions?.Count ?? 0; // Avoid nulls
                    var difference = draft.QuestionsAmountTarget - questionCount; // Assuming TotalQuestionsNeeded returns the total number of questions needed

                    if (difference > 0 && difference > maxDifference)
                    {
                        draftWithGreatestDifference = draft;
                        maxDifference = difference;
                        Console.WriteLine($"{draft} id: {draft.Id} -- good. Total questions found: {questionCount}, difference: {difference}.");
                    }
                    else
                    {
                        //Console.WriteLine($"{draft} id: {draft.Id} -- bad. Trying again... Total questions found: {questionCount}, difference: {difference}.");
                    }
                }

                if (draftWithGreatestDifference != null)
                {
                    Console.WriteLine($"Final decision: Draft id: {draftWithGreatestDifference.Id}, Total questions found: {draftWithGreatestDifference.Questions?.Count ?? 0}, Questions needed: {draftWithGreatestDifference.QuestionsAmountTarget}");
                }
                else
                {
                    Console.WriteLine("No suitable draft found.");
                }

                return draftWithGreatestDifference;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetNextDraftToMakeQuestionsAsync: An exception occurred while retrieving the next draft to make questions:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void EnsureDraftNotNull()
        {
            if (_context.Drafts == null)
            {
                throw new InvalidOperationException("DraftRepository/EnsureDraftNotNull: The Draft DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
