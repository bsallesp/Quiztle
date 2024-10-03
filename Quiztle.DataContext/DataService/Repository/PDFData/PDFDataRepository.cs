using Quiztle.CoreBusiness.Entities.PDFData;
using Microsoft.EntityFrameworkCore;

namespace Quiztle.DataContext.Repositories.Quiz
{
    public class PDFDataRepository(PostgreQuiztleContext context)
    {
        private readonly PostgreQuiztleContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task CreatePDFDataAsync(PDFData pdfData)
        {
            _context.PDFData!.Add(pdfData);
            await _context.SaveChangesAsync();
        }

        public async Task<PDFData?> GetPDFDataByIdAsync(Guid id)
        {
            return await _context.PDFData!
                .Include(p => p.Pages)
                .FirstOrDefaultAsync(pdf => pdf.Id == id);
        }

        public async Task UpdatePDFDataAsync(PDFData pdfData)
        {
            _context.PDFData!.Update(pdfData);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePDFDataAsync(Guid id)
        {
            var pdfData = await GetPDFDataByIdAsync(id);
            if (pdfData != null)
            {
                _context.PDFData!.Remove(pdfData);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<PDFData>> GetAllPDFDataAsync()
        {
            return await _context.PDFData!.
                Include(p => p.Pages)
                .Include(t => t.Tests)
                .ThenInclude(q => q.Questions)
                .ToListAsync();
        }

        public async Task<PDFData?> GetPDFDataByIdAsyncByPage(Guid id, int startPage = 0, int endPage = 0)
        {
            if (startPage == 0 && endPage == 0)
            {
                return await _context.PDFData!
                    .Include(p => p.Pages.OrderBy(page => page.Page))
                    .FirstOrDefaultAsync(pdf => pdf.Id == id);
            }

            int actualStartPage = Math.Max(1, startPage);

            int actualEndPage = endPage == 0 ? int.MaxValue : endPage;

            var pdfData = await _context.PDFData!
                .Include(p => p.Pages)
                .FirstOrDefaultAsync(pdf => pdf.Id == id);

            if (pdfData == null) return null;

            var filteredPages = pdfData.Pages
                .Where(page => page.Page >= actualStartPage && page.Page <= actualEndPage)
                .OrderBy(page => page.Page)
                .ToList();

            return new PDFData
            {
                Id = pdfData.Id,
                Pages = filteredPages
            };
        }

        //public async Task<PDFData?> GetPDFDataByIdAsync(Guid id)
        //{
        //    return await _context.PDFData!
        //        .Include(p => p.Pages)
        //        .FirstOrDefaultAsync(pdf => pdf.Id == id);
        //}

        //public async Task<List<PDFData>> GetAllPDFDataAsync()
        //{
        //    EnsureOptionsNotNull();
        //    return await _context.PDFData!.ToListAsync();
        //}

        private void EnsureOptionsNotNull()
        {
            if (_context.Options == null)
            {
                throw new InvalidOperationException("The Options DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
