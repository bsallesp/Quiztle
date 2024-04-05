using Microsoft.EntityFrameworkCore;
using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.DataContext.DataService.Repository.Course
{
    public class TopicClassRepository
    {
        private readonly PostgreBrunoTheBotContext _context;

        public TopicClassRepository(PostgreBrunoTheBotContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateTopicAsync(TopicClass topic)
        {
            try
            {
                EnsureTopicsNotNull();
                _context.TopicClasses!.Add(topic);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the topic:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<TopicClass?> GetTopicByIdAsync(int id)
        {
            EnsureTopicsNotNull();
            return await _context.TopicClasses!.FindAsync(id);
        }

        public async Task<IQueryable<TopicClass>> GetAllTopicsAsync()
        {
            EnsureTopicsNotNull();
            var topics = await _context.TopicClasses!.ToListAsync();
            return topics.AsQueryable();
        }

        public async Task UpdateTopicAsync(TopicClass topic)
        {
            EnsureTopicsNotNull();
            _context.Entry(topic).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTopicAsync(int id)
        {
            EnsureTopicsNotNull();
            var topic = await _context.TopicClasses!.FindAsync(id);
            if (topic != null)
            {
                _context.TopicClasses.Remove(topic);
                await _context.SaveChangesAsync();
            }
        }

        private void EnsureTopicsNotNull()
        {
            if (_context.TopicClasses == null)
            {
                throw new InvalidOperationException("The Topics DbSet is null. Make sure it is properly initialized.");
            }
        }
    }
}
