using Quiztle.API.Controllers.Tasks.Engines;
using Quiztle.DataContext.DataService.Repository.Tasks;

namespace Quiztle.API.BackgroundTasks
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TryToMoveBookTaskToProduction _tryToMoveBookTaskToProduction;

        public TimedHostedService(IServiceScopeFactory scopeFactory, TryToMoveBookTaskToProduction tryToMoveBookTaskToProduction)
        {
            _scopeFactory = scopeFactory;
            _tryToMoveBookTaskToProduction = tryToMoveBookTaskToProduction;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork!, null, TimeSpan.Zero,
                TimeSpan.FromMilliseconds(100000000));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var bookTaskRepository = scope.ServiceProvider.GetRequiredService<BookTaskRepository>();
                try
                {
                    var result = await _tryToMoveBookTaskToProduction.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An exception occurred while checkin tht queue: ");
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
