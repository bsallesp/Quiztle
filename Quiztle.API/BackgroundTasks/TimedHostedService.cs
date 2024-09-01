using Quiztle.API.BackgroundTasks.Questions;

namespace Quiztle.API.BackgroundTasks
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public TimedHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Define o intervalo do Timer
            _timer = new Timer(DoWork!, null, TimeSpan.Zero, TimeSpan.FromSeconds(150));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            // Cria um escopo para usar serviços scoped
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var bgQuestions = scope.ServiceProvider.GetRequiredService<BGQuestions>();

                    Console.WriteLine("Launching _bgQuestions.ExecuteAsync... at " + DateTime.UtcNow);
                    var result = await bgQuestions.ExecuteAsync();
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("TimedHostedService / DoWork: An exception occurred while checking the queue: ");
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Para o Timer quando o serviço for interrompido
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Libera os recursos do Timer
            _timer?.Dispose();
        }
    }
}
