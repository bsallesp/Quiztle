using BrunoTheBot.API;
using BrunoTheBot.API.Controllers.LLMControllers;
using BrunoTheBot.API.Controllers.HeadControllers.Create;
using BrunoTheBot.DataContext;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using BrunoTheBot.DataContext.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os HTTP Client e Controllers
builder.Services.AddHttpClient<IChatGPTRequest, ChatGPTRequest>();
builder.Services.AddScoped<AILogRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<LogController>();
builder.Services.AddScoped<CreateBook>();
builder.Services.AddTransient<GetChaptersFromLLM>();
builder.Services.AddTransient<GetContentFromLLLM>();
builder.Services.AddTransient<GetQuestionsFromLLM>();
builder.Services.AddTransient<GetAllBookSectionsFromLLM>();

builder.Services.AddControllers();

// Configura��o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Configura��o do DbContext
builder.Services.AddDbContextFactory<PostgreBrunoTheBotContext>(opt =>
{
    string connectionString = ConnectionStrings.DevelopmentConnectionString;
    var env = builder.Environment;
    if (env.IsProduction()) connectionString = ConnectionStrings.ProductionConnectionString;
    opt.UseNpgsql(connectionString);
});

// Configura��o do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Habilita CORS
app.UseCors("AllowAnyOrigin");

// Configura��es adicionais do ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapeia os controllers
app.MapControllers();

app.Run();
