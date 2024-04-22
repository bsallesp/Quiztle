using BrunoTheBot.Blazor.Client;
using BrunoTheBot.Blazor.Client.APIServices;
using BrunoTheBot.Blazor.Client.APIServices.RegularGame;
using BrunoTheBot.Blazor.Client.Authentication.Core;
using BrunoTheBot.DataContext;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient<BookRepository>();
builder.Services.AddTransient<GetAllBooksService>();
builder.Services.AddTransient<RetrieveBookByIdService>();
builder.Services.AddTransient<GetAllQuestionsToRegularGame>();
builder.Services.AddTransient<CheckRenderSide>();

builder.Services.AddScoped<DefaultAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp
    => sp.GetRequiredService<DefaultAuthenticationStateProvider>());
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

string localAPIURLString = builder.Configuration["localAPIURL"] ?? throw new Exception("Connection string not found.");
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(localAPIURLString ?? "https://localhost:7204/")
    });

#region snippet1
string localDbConnection = builder.Configuration["ConnectionString"] ?? throw new Exception("Connection string not found.");
builder.Services.AddDbContextFactory<PostgreBrunoTheBotContext>(opt =>
{
    Console.WriteLine(localDbConnection);
    opt.UseNpgsql(localDbConnection);
});
#endregion

await builder.Build().RunAsync();