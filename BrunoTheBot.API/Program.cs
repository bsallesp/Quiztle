using BrunoTheBot.API;
using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.DataContext;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using BrunoTheBot.DataContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BrunoTheBot.API.BackgroundTasks;
using BrunoTheBot.API.Controllers.CourseControllers.BookControllers;
using BrunoTheBot.DataContext.DataService.Repository.Tasks;
using BrunoTheBot.API.Controllers.Tasks.Engines;
using BrunoTheBot.API.Controllers.Tasks;
using BrunoTheBot.API.Services;
using BrunoTheBot.API.Controllers.CourseControllers.QuestionControllers;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.API.Controllers.PDFApi;
using BrunoTheBot.DataContext.DataService.Repository.Quiz;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IChatGPTRequest, ChatGPTRequest>();
builder.Services.AddTransient<ChatGPTRequest>();

builder.Services.AddTransient(typeof(LogService<>));

builder.Services.AddTransient<SaveAILogController>();
builder.Services.AddTransient<CreateBookController>();
builder.Services.AddTransient<CreateBookTaskController>();
builder.Services.AddTransient<CreateTestFromPDFDataPages>();

builder.Services.AddTransient<CreateQuestionsFromBookController>();

builder.Services.AddTransient<GetChaptersFromLLM>();
builder.Services.AddTransient<GetContentFromLLLM>();

builder.Services.AddTransient<GetQuestionsFromLLM>();

builder.Services.AddTransient<GetAllBookSectionsFromLLM>();

builder.Services.AddTransient<AILogRepository>();
builder.Services.AddTransient<BookRepository>();
builder.Services.AddTransient<BookTaskRepository>();
builder.Services.AddTransient<PDFDataRepository>();
builder.Services.AddTransient<QuestionRepository>();
builder.Services.AddTransient<TestRepository>();

builder.Services.AddTransient<TryToMoveBookTaskToProduction>();

builder.Services.AddHostedService<TimedHostedService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

#region pdfApiUrl
var pdfApiUrl = Environment.GetEnvironmentVariable("PDF_API_URL");
if (string.IsNullOrEmpty(pdfApiUrl)) pdfApiUrl = builder.Configuration["PDF_API_URL"];
if (string.IsNullOrEmpty(pdfApiUrl)) throw new Exception("Cant get PDF_API_URL at webassembly");
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(pdfApiUrl),
    Timeout = Timeout.InfiniteTimeSpan
});
#endregion

string connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ?? "";
if (connectionString.IsNullOrEmpty()) connectionString = builder.Configuration["ConnectionString"]!;
if (connectionString.IsNullOrEmpty()) throw new Exception("Cant get connections at webcoreapi.");

builder.Services.AddDbContextFactory<PostgreBrunoTheBotContext>(opt =>
{
    opt.UseNpgsql(connectionString);
},ServiceLifetime.Scoped);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAnyOrigin");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHsts();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();