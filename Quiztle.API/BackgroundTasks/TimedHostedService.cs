using Microsoft.Extensions.Configuration;
using Quiztle.API.BackgroundTasks.Questions;

namespace Quiztle.API.BackgroundTasks
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public TimedHostedService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        private int GetHostTime()
        {
            var iHostTime = Environment.GetEnvironmentVariable("IHOST_TIME");
            if (!string.IsNullOrEmpty(iHostTime) && int.TryParse(iHostTime, out int seconds))
            {
                Console.WriteLine("TimedHostedService: IHOST_TIME found in environment variables.");
                return seconds;
            }

            Console.WriteLine("TimedHostedService: Environment Variable IHOST_TIME NOT FOUND. Checking appsettings...");

            iHostTime = _configuration["IHOST_TIME"];
            if (!string.IsNullOrEmpty(iHostTime) && int.TryParse(iHostTime, out seconds))
            {
                Console.WriteLine("TimedHostedService: IHOST_TIME found in appsettings.");
                return seconds;
            }

            throw new Exception("TimedHostedService: IHOST_TIME not found in both environment variables and appsettings.");
        }

        private bool IsHostActive()
        {
            var isHostActive = Environment.GetEnvironmentVariable("IS_IHOST_ACTIVE");
            if (!string.IsNullOrEmpty(isHostActive))
            {
                Console.WriteLine("TimedHostedService: IS_IHOST_ACTIVE found in environment variables.");
                return bool.Parse(isHostActive);
            }

            Console.WriteLine("TimedHostedService: Environment Variable IS_IHOST_ACTIVE NOT FOUND. Checking appsettings...");

            isHostActive = _configuration["IS_IHOST_ACTIVE"];
            if (!string.IsNullOrEmpty(isHostActive))
            {
                Console.WriteLine("TimedHostedService: IS_IHOST_ACTIVE found in appsettings.");
                return bool.Parse(isHostActive);
            }

            throw new Exception("TimedHostedService: IS_IHOST_ACTIVE not found in both environment variables and appsettings.");
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            var seconds = GetHostTime();
            if (seconds == -1 || !IsHostActive())
            {
                return Task.CompletedTask;
            }

            _timer = new Timer(DoWork!, null, TimeSpan.Zero, TimeSpan.FromSeconds(seconds));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var bgQuestions = scope.ServiceProvider.GetRequiredService<BuildQuestionsInBackgroundByLLM>();

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
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
