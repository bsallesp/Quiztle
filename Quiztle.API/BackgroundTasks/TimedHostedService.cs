using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quiztle.API.BackgroundTasks.Curation;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.DataContext.Repositories.Quiz;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var questionRepository = scope.ServiceProvider.GetRequiredService<QuestionRepository>();
                    var llmRequest = scope.ServiceProvider.GetRequiredService<ILLMRequest>();
                    var curationBackground = new CurationBackground(llmRequest, CancellationToken.None); // Adjust as needed

                    var getQuestionRate = new GetQuestionRate(questionRepository, curationBackground);
                    await getQuestionRate.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("TimedHostedService / DoWork: An exception occurred: ");
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
