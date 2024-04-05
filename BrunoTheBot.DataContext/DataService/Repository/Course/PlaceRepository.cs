//using Microsoft.EntityFrameworkCore;
//using BrunoTheBot.CoreBusiness.Entities.Course;

//namespace BrunoTheBot.DataContext.DataService.Repository.Course
//{
//    public class PlaceRepository
//    {
//        private readonly PostgreBrunoTheBotContext _context;

//        public PlaceRepository(PostgreBrunoTheBotContext context)
//        {
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//        }

//        public async Task CreatePlaceAsync(TopicClass topic)
//        {
//            try
//            {
//                EnsureTopicsNotNull();
//                _context.pl!.Add(topic);
//                await _context.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("An exception occurred while creating the topic:");
//                Console.WriteLine(ex.ToString());
//                throw;
//            }
//        }

//        public async Task<TopicClass?> GetTopicByIdAsync(int id)
//        {
//            EnsureTopicsNotNull();
//            return await _context.Topics!.FindAsync(id);
//        }

//        public async Task<IQueryable<TopicClass>> GetAllTopicsAsync()
//        {
//            EnsureTopicsNotNull();
//            var topics = await _context.Topics!.ToListAsync();
//            return topics.AsQueryable();
//        }

//        public async Task UpdateTopicAsync(TopicClass topic)
//        {
//            EnsureTopicsNotNull();
//            _context.Entry(topic).State = EntityState.Modified;
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteTopicAsync(int id)
//        {
//            EnsureTopicsNotNull();
//            var topic = await _context.Topics!.FindAsync(id);
//            if (topic != null)
//            {
//                _context.Topics.Remove(topic);
//                await _context.SaveChangesAsync();
//            }
//        }

//        private void EnsureTopicsNotNull()
//        {
//            if (_context.Topics == null)
//            {
//                throw new InvalidOperationException("The Topics DbSet is null. Make sure it is properly initialized.");
//            }
//        }
//    }
//}
