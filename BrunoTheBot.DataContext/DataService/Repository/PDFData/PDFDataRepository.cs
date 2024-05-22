using BrunoTheBot.CoreBusiness.Entities.PDFData;

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

        public async Task<PDFData?> GetPDFDataByIdAsync(Guid id)
        {
            EnsureOptionsNotNull();
            return await _context.PDFData!.FindAsync(id);
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
