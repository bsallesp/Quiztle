using BrunoTheBot.Blazor.APIServices;
using BrunoTheBot.DataContext;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddScoped<GetAllSchoolsService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7204/") // Ou "http://localhost:5044/"
});


#region snippet1
builder.Services.AddDbContextFactory<PostgreBrunoTheBotContext>(opt =>
{
    string connectionString = ConnectionStrings.DevelopmentConnectionString;

    var env = builder.HostEnvironment;
    if (env.IsProduction()) connectionString = ConnectionStrings.ProductionConnectionString;

    Console.WriteLine(connectionString);
    opt.UseNpgsql(connectionString);
});
#endregion

await builder.Build().RunAsync();
