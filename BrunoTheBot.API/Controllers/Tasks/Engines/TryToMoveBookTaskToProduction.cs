using BrunoTheBot.API.Controllers.CourseControllers.BookControllers;
using BrunoTheBot.DataContext;
using BrunoTheBot.DataContext.DataService.Repository.Tasks;

namespace BrunoTheBot.API.Controllers.Tasks.Engines
{
    public class TryToMoveBookTaskToProduction
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TryToMoveBookTaskToProduction(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> ExecuteAsync()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PostgreBrunoTheBotContext>();
                    var bookTaskRepository = scope.ServiceProvider.GetRequiredService<BookTaskRepository>();
                    var createBookController = scope.ServiceProvider.GetRequiredService<CreateBookController>();

                    var bookNameToProduction = await bookTaskRepository.GetNextBookTaskFromQueue();
                    if (bookNameToProduction == "") return false;
                    else await createBookController.ExecuteAsync(bookNameToProduction);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the BookTask:");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}