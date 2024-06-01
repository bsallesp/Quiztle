using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities.Quiz;

namespace BrunoTheBot.DataContext.DataService.Repository.Quiz
{
    public class ResponseRepository
    {
        private readonly DbContext _context;

        public ResponseRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Response> GetByIdAsync(Guid id)
        {
            return await _context.Set<Response>().FindAsync(id) ?? new Response();
        }

        public async Task<IEnumerable<Response>> GetAllAsync()
        {
            return await _context.Set<Response>().ToListAsync();
        }

        public async Task AddAsync(Response response)
        {
            await _context.Set<Response>().AddAsync(response);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Response response)
        {
            _context.Set<Response>().Update(response);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await GetByIdAsync(id);
            if (response != null)
            {
                _context.Set<Response>().Remove(response);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByTestIdAsync(Guid testId)
        {
            return await _context.Set<Response>().AnyAsync(r => r.Test.Id == testId);
        }
    }
}
