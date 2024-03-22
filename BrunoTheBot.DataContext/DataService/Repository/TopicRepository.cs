using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness;

namespace BrunoTheBot.DataContext.DataService.Repository
{
    public class TopicRepository
    {
        private readonly SqliteDataContext _context;

        public TopicRepository(SqliteDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateTopicAsync(Topic topic)
        {
            try
            {
                EnsureTopicsNotNull();
                _context.Topics!.Add(topic);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the topic:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Topic?> GetTopicByIdAsync(int id)
        {
            EnsureTopicsNotNull();
            return await _context.Topics.FindAsync(id);
        }

        public async Task<IQueryable<Topic>> GetAllTopicsAsync()
        {
            EnsureTopicsNotNull();
            return _context.Topics.AsQueryable();
        }

        public async Task UpdateTopicAsync(Topic topic)
        {
            EnsureTopicsNotNull();
            _context.Entry(topic).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTopicAsync(int id)
        {
            EnsureTopicsNotNull();
            var topic = await _context.Topics.FindAsync(id);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureTopicsNotNull()
        {
            if (_context.Topics == null)
            {
                throw new InvalidOperationException("The Topics DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
