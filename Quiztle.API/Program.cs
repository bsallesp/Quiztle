using Quiztle.API;
using Quiztle.API.Controllers.LLMControllers;
using Quiztle.DataContext;
using Quiztle.DataContext.DataService.Repository.Course;
using Quiztle.DataContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Quiztle.API.BackgroundTasks;
using Quiztle.API.Controllers.CourseControllers.BookControllers;
using Quiztle.DataContext.DataService.Repository.Tasks;
using Quiztle.API.Controllers.Tasks.Engines;
using Quiztle.API.Controllers.Tasks;
using Quiztle.API.Services;
using Quiztle.API.Controllers.CourseControllers.QuestionControllers;
using Quiztle.DataContext.Repositories.Quiz;
using Quiztle.API.Controllers.PDFApi;
using Quiztle.DataContext.DataService.Repository.Quiz;
using System.Text.Json.Serialization;
using Quiztle.API.BackgroundTasks.Questions;
using Quiztle.API.Controllers.LLM;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.API.Controllers.ScratchControllers;
using Quiztle.DataContext.DataService.Repository;
using Quiztle.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<OllamaRequest>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddHttpClient<FromPDFToJsonFile>();

builder.Services.AddTransient<ILLMChatGPTRequest, ChatGPTRequest>();
builder.Services.AddTransient<IEndpointProvider, NgrokEndpointProvider>();
builder.Services.AddTransient<ILLMRequest, OllamaRequest>();

builder.Services.AddTransient<RemoveBadQuestions>();

builder.Services.AddTransient(typeof(LogService<>));

//builder.Services.AddTransient<SaveAILogController>();
builder.Services.AddTransient<CreateBookByLLMController>();
builder.Services.AddTransient<CreateBookTaskController>();
builder.Services.AddTransient<CreateTestFromPDFDataPages>();
builder.Services.AddTransient<AddScratchController>();
builder.Services.AddTransient<CreateQuestionsFromBookController>();
builder.Services.AddTransient<CreateAllTestsByScratchController>();
builder.Services.AddTransient<CreateTestByDraftController>();
builder.Services.AddTransient<CreateTestByScratchController>();
builder.Services.AddTransient<GetAllScratchesController>();

builder.Services.AddTransient<GetChaptersFromLLM>();
builder.Services.AddTransient<GetContentFromLLLM>();

builder.Services.AddTransient<GetQuestionsFromLLM>();

builder.Services.AddTransient<GetAllBookSectionsFromLLM>();

builder.Services.AddTransient<AILogRepository>();
builder.Services.AddTransient<BookRepository>();
builder.Services.AddTransient<BookTaskRepository>();
builder.Services.AddTransient<PDFDataRepository>();
builder.Services.AddTransient<PromptRepository>();
builder.Services.AddTransient<QuestionRepository>();
builder.Services.AddTransient<ResponseRepository>();
builder.Services.AddTransient<ScratchRepository>();
builder.Services.AddTransient<ShotRepository>();
builder.Services.AddTransient<TestRepository>();
builder.Services.AddTransient<DraftRepository>();
builder.Services.AddTransient<UserRepository>();

builder.Services.AddTransient<BuildQuestionsInBackgroundByLLM>();

builder.Services.AddTransient<TryToMoveBookTaskToProduction>();

builder.Services.AddHostedService<TimedHostedService>();

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

#region pdfApiUrl
try
{
    var pdfApiUrl = Environment.GetEnvironmentVariable("PROD_FLASK_API_URL") ?? builder.Configuration["DevelopmentPDF_API_URL"];
    builder.Services.AddHttpClient("PDFClient", client =>
    {

        if (string.IsNullOrEmpty(pdfApiUrl))
        {
            throw new InvalidOperationException("PDF API URL is not set in environment variables or configuration.");
        }

        client.BaseAddress = new Uri(pdfApiUrl);
        client.Timeout = TimeSpan.FromDays(1);
    });
    Console.WriteLine($"PDFClient configured with base URL: {pdfApiUrl}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error configuring PDFClient: {ex}");
}
#endregion

#region Postgresql Connection
var connectionString = builder.Configuration["PROD_POSTGRES_CONNECTION_STRING"];
if (string.IsNullOrEmpty(connectionString)) throw new Exception($"Cant get connections at Quiztle.API project: {connectionString}");
Console.WriteLine($"Connection gotten: {connectionString}");
#endregion

builder.Services.AddDbContextFactory<PostgreQuiztleContext>(opt =>
{
    opt.UseNpgsql(connectionString);
},ServiceLifetime.Scoped);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHsts();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();