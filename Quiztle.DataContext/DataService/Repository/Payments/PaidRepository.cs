using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Entities.Paid;

namespace Quiztle.DataContext.DataService.Repository.Payments
{
    public class PaidRepository
    {
        private readonly PostgreQuiztleContext _context;
        
        public PaidRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task CreatePaidAsync(Paid paid)
        {
            try
            {
                EnsurePaidNotNull();
                _context.Paids!.Add(paid);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreatePaidAsync: An exception occurred while creating the paid entry:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        public async Task<bool> IsPaid(Paid paid)
        {
            try
            {
                EnsurePaidNotNull();

                var test = await _context.Tests!.Where(x => x.Id.ToString() == paid.TestId).FirstOrDefaultAsync();
                if (test == null) return false;
                
                paid.PriceId = test.PriceId;
                Console.WriteLine($"Test / PriceId: {paid.PriceId}, {paid.UserEmail}, {paid.TestId}");
                
                var result = await _context.Paids!
                    .Where(e => e.UserEmail == paid.UserEmail && e.PriceId == paid.PriceId)
                    .OrderByDescending(e => e.Created)
                    .FirstOrDefaultAsync();

                return result != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("IsPaid: An exception occurred while retrieving the paid entry by Id:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        public async Task<Paid?> GetPaidByIdAsync(Guid id)
        {
            try
            {
                EnsurePaidNotNull();
                return await _context.Paids!
                    .AsTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPaidByIdAsync: An exception occurred while retrieving the paid entry by Id:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        public async Task<IEnumerable<Paid>> GetAllPaidAsync()
        {
            try
            {
                EnsurePaidNotNull();
                return await _context.Paids!
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllPaidAsync: An exception occurred while retrieving all paid entries:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        public async Task<IEnumerable<Paid>> GetPaidByUserIdAsync(Guid userId)
        {
            try
            {
                EnsurePaidNotNull();
                return await _context.Paids!
                    .Where(p => p.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPaidByUserIdAsync: An exception occurred while retrieving paid entries by user ID:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<Paid>> GetPaidByEmailAsync(string email)
        {
            try
            {
                EnsurePaidNotNull();
                return await _context.Paids!
                    .Where(e => e.UserEmail == email)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPaidByUserIdAsync: An exception occurred while retrieving paid entries by user ID:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        public async Task UpdatePaidAsync(Paid paid)
        {
            try
            {
                EnsurePaidNotNull();

                var existingPaid = await _context.Paids!
                    .FirstOrDefaultAsync(p => p.Id == paid.Id);

                if (existingPaid == null)
                {
                    throw new InvalidOperationException("UpdatePaidAsync: The paid entry to update does not exist.");
                }

                _context.Entry(existingPaid).CurrentValues.SetValues(paid);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdatePaidAsync: An exception occurred while updating the paid entry:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        private void EnsurePaidNotNull()
        {
            if (_context.Paids == null)
            {
                throw new InvalidOperationException("PaidRepository/EnsurePaidNotNull: The Paid DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
