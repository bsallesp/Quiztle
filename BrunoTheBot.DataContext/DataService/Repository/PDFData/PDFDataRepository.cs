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

        //public async Task<PDFData?> GetPDFDataByIdAsync(Guid id)
        //{
        //    EnsureOptionsNotNull();
        //    return await _context.PDFData!.FindAsync(id);
        //}

        public async Task<PDFData?> GetPDFDataByIdAsyncByPage(Guid id, int startPage = 1, int endPage = int.MaxValue)
        {
            if (startPage == 1 && endPage == int.MaxValue)
            {
                return await _context.PDFData!
                    .Include(p => p.Pages.OrderBy(page => page.Page))  // Ordena as páginas
                    .FirstOrDefaultAsync(pdf => pdf.Id == id);
            }

            return await _context.PDFData!
                .Include(p => p.Pages.Where(page => page.Page >= startPage && page.Page <= endPage)
                                     .OrderBy(page => page.Page))  // Ordena as páginas dentro do intervalo especificado
                .FirstOrDefaultAsync(pdf => pdf.Id == id);
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
