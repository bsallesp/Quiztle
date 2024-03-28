using BrunoTheBot.APIs;
using BrunoTheBot.Blazor.APIServices;
using BrunoTheBot.DataContext;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient<HuggingFaceAPI>();
builder.Services.AddTransient<DeepSeekAPI>();
builder.Services.AddTransient<AILogService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7204/") // Ou "http://localhost:5044/"
});


#region snippet1
builder.Services.AddDbContextFactory<SqliteDataContext>(opt =>
{
    string connectionString = ConnectionStrings.DevelopmentConnectionString;

    var env = builder.HostEnvironment;
    if (env.IsProduction()) connectionString = ConnectionStrings.ProductionConnectionString;

    Console.WriteLine(connectionString);
    opt.UseSqlite(connectionString);
});
#endregion

await builder.Build().RunAsync();
