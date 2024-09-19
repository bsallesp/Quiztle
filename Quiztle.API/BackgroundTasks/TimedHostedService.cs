using Quiztle.API.BackgroundTasks.Curation;
using Quiztle.API.BackgroundTasks.Questions;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.DataContext.Repositories;
using Quiztle.DataContext.Repositories.Quiz;
using Quiztle.DataContext.DataService.Repository.Quiz;
using Quiztle.DataContext.DataService.Repository;

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
            // Ajuste os intervalos dos timers conforme necessário
            _timer = new Timer(DoCreateQuestionsWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            //_timer = new Timer(DoCurationWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private async void DoCurationWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var questionRepository = scope.ServiceProvider.GetRequiredService<QuestionRepository>();
                    var llmRequest = scope.ServiceProvider.GetRequiredService<ILLMRequest>();
                    var removeBadQuestions = scope.ServiceProvider.GetService<RemoveBadQuestions>();

                    var curationBackground = new CurationBackground(
                        llmRequest, CancellationToken.None, removeBadQuestions!);

                    var getQuestionRate = new GetQuestionRate(questionRepository, curationBackground);
                    await getQuestionRate.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("TimedHostedService / DoCurationWork: An exception occurred: ");
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private async void DoCreateQuestionsWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var llmRequest = scope.ServiceProvider.GetRequiredService<ILLMRequest>();
                    var aILogRepository = scope.ServiceProvider.GetRequiredService<AILogRepository>();
                    var testRepository = scope.ServiceProvider.GetRequiredService<TestRepository>();
                    var draftRepository = scope.ServiceProvider.GetRequiredService<DraftRepository>();
                    var questionRepository = scope.ServiceProvider.GetRequiredService<QuestionRepository>();

                    var buildQuestions = new BuildQuestionsInBackgroundByLLM(
                        llmRequest,
                        aILogRepository,
                        testRepository,
                        draftRepository,
                        questionRepository
                    );

                    await buildQuestions.ExecuteAsync();

                    //// Após a criação das perguntas, remova duplicatas
                    //var removeDuplicates = new RemoveDuplicates(questionRepository);
                    //await removeDuplicates.ExecuteAsync();

                    //// Remover perguntas com rate < 3
                    //var removeBadQuestions = new RemoveBadQuestions(questionRepository);
                    //await removeBadQuestions.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("TimedHostedService / DoCreateQuestionsWork: An exception occurred: ");
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
