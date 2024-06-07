using BrunoTheBot.API.Controllers.CourseControllers.BookControllers;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Utils;
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

        public async Task<APIResponse<bool>> ExecuteAsync()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PostgreBrunoTheBotContext>();
                    var bookTaskRepository = scope.ServiceProvider.GetRequiredService<BookTaskRepository>();
                    var createBookController = scope.ServiceProvider.GetRequiredService<CreateBookController>();

                    var GetNextBookTaskFromQueueResult = await bookTaskRepository.GetNextBookTaskFromQueue();
                    if (GetNextBookTaskFromQueueResult.Status == CustomStatusCodes.ErrorStatus ||
                        GetNextBookTaskFromQueueResult.Status == CustomStatusCodes.NotFound)
                        return new APIResponse<bool>
                        {
                            Status = CustomStatusCodes.ErrorStatus,
                            Data = false,
                            Message = GetNextBookTaskFromQueueResult.Status
                        };

                    Console.WriteLine(GetNextBookTaskFromQueueResult.Status.ToString());
                    Console.WriteLine(GetNextBookTaskFromQueueResult.Data.Name);
                    Console.WriteLine(GetNextBookTaskFromQueueResult.Data.Id);
                    Console.WriteLine(GetNextBookTaskFromQueueResult.Data.Created);

                    var createBookControllerResult = await createBookController.ExecuteAsync(GetNextBookTaskFromQueueResult.Data.Name!, 5, 5);
                    var finishTaskResult = await bookTaskRepository.FinishBookTask(createBookControllerResult.Value!.Data.Id);

                    return new APIResponse<bool>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = true,
                        Message = finishTaskResult.Message
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while creating the BookTask:");
                Console.WriteLine(ex.ToString());
                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = ex.Data.ToString()
                };
            }
        }
    }
}