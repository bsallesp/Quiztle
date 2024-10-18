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
        private readonly IHostEnvironment _hostEnvironment;

        public TimedHostedService(IServiceScopeFactory scopeFactory, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                _timer = new Timer(DoCreateQuestionsWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
                //_timer = new Timer(DoCurationWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            }

            return Task.CompletedTask;
        }

        private async void DoCurationWork(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    Console.WriteLine("Launching CurationWork... ");
                    var questionRepository = scope.ServiceProvider.GetRequiredService<QuestionRepository>();
                    var llmRequest = scope.ServiceProvider.GetRequiredService<ILLMChatGPTRequest>();
                    var answerValidateQuestions = scope.ServiceProvider.GetRequiredService<AnswerValidateQuestions>();

                    // Agora, crie a instância de curationBackground antes de usá-la.
                    var curationBackground = new CurationBackground(
                        llmRequest, CancellationToken.None, answerValidateQuestions);

                    // Agora, você pode criar a instância de GetQuestionRate e passar curationBackground.
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
                    var llmRequest = scope.ServiceProvider.GetRequiredService<ILLMChatGPTRequest>();
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
