using BrunoTheBot.API;
using BrunoTheBot.DataContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os HTTP Client e Controllers
builder.Services.AddHttpClient<IChatGPTRequest, ChatGPTRequest>();
builder.Services.AddScoped<AILogRepository>();
builder.Services.AddScoped<SchoolRepository>();
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
builder.Services.AddDbContextFactory<SqliteDataContext>(opt =>
{
    string connectionString = ConnectionStrings.DevelopmentConnectionString;

    var env = builder.Environment;
    if (env.IsProduction()) connectionString = ConnectionStrings.ProductionConnectionString;

    Console.WriteLine(connectionString);
    opt.UseSqlite(connectionString);
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