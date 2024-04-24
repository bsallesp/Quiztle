using BrunoTheBot.API;
using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.API.Controllers.HeadControllers.Create;
using BrunoTheBot.DataContext;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using BrunoTheBot.DataContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços HTTP Client e Controllers
builder.Services.AddHttpClient<IChatGPTRequest, ChatGPTRequest>();
builder.Services.AddScoped<AILogRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<LogController>();
builder.Services.AddScoped<CreateBookController>();
builder.Services.AddTransient<GetChaptersFromLLM>();
builder.Services.AddTransient<GetContentFromLLLM>();
builder.Services.AddTransient<GetQuestionsFromLLM>();
builder.Services.AddTransient<GetAllBookSectionsFromLLM>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

string connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ?? "";
if (connectionString.IsNullOrEmpty()) connectionString = builder.Configuration["ConnectionString"]!;
if (connectionString.IsNullOrEmpty()) throw new Exception("Cant get connections");

builder.Services.AddDbContextFactory<PostgreBrunoTheBotContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();


