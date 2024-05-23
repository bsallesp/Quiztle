using BrunoTheBot.CoreBusiness.Entities.PDFData;
using Microsoft.EntityFrameworkCore;

namespace BrunoTheBot.DataContext.Repositories.Quiz
{
    public class PDFDataRepository(PostgreBrunoTheBotContext context)
    {
        private readonly PostgreBrunoTheBotContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task CreatePDFDataAsync(PDFData pdfData)
        {
            try
            {
                EnsureOptionsNotNull();
                _context.PDFData!.Add(pdfData);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the pdfData:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<PDFData?> GetPDFDataByIdAsyncByPage(Guid id, int startPage = 0, int endPage = 0)
        {
            if (startPage == 0 && endPage == 0)
            {
                // Se ambos startPage e endPage são 0, retorna todas as páginas
                return await _context.PDFData!
                    .Include(p => p.Pages.OrderBy(page => page.Page))
                    .FirstOrDefaultAsync(pdf => pdf.Id == id);
            }

            // Ajusta startPage para ser no mínimo 1
            int actualStartPage = Math.Max(1, startPage);

            // Se endPage não é 0, utiliza-o; caso contrário, usa int.MaxValue para representar 'sem limite'
            int actualEndPage = endPage == 0 ? int.MaxValue : endPage;

            var pdfData = await _context.PDFData!
                .Include(p => p.Pages)
                .FirstOrDefaultAsync(pdf => pdf.Id == id);

            if (pdfData == null) return null;

            // Filtra e ordena as páginas na memória para garantir que não tentemos acessar páginas fora do alcance
            pdfData.Pages = pdfData.Pages
                .Where(page => page.Page >= actualStartPage && page.Page <= actualEndPage)
                .OrderBy(page => page.Page)
                .ToList();

            return pdfData;
        }


        public async Task<PDFData?> GetPDFDataByIdAsync(Guid id)
        {
            return await _context.PDFData!
                .Include(p => p.Pages)
                .FirstOrDefaultAsync(pdf => pdf.Id == id);
        }

        public async Task<List<PDFData>> GetAllPDFDataAsync()
        {
            EnsureOptionsNotNull();
            return await _context.PDFData!.ToListAsync();
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
