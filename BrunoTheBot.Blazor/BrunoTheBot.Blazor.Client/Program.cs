using BrunoTheBot.Blazor.Client;
using BrunoTheBot.Blazor.Client.APIServices;
using BrunoTheBot.Blazor.Client.APIServices.RegularGame;
using BrunoTheBot.Blazor.Client.Authentication.Core;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using BrunoTheBot.DataContext.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient<BookRepository>();
builder.Services.AddTransient<GetAllBooksService>();
builder.Services.AddTransient<RetrieveBookByIdService>();
builder.Services.AddTransient<GetAllQuestionsToRegularGame>();
builder.Services.AddTransient<CheckRenderSide>();
builder.Services.AddTransient<CreateBookTaskService>();

builder.Services.AddTransient<AILogRepository>();

builder.Services.AddScoped<DefaultAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp
    => sp.GetRequiredService<DefaultAuthenticationStateProvider>());
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

#region connectionString
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString)) connectionString = builder.Configuration["ConnectionString"]!;
if (string.IsNullOrEmpty(connectionString)) throw new Exception("Cant get connections at webassembly");
#endregion

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region apiBaseUrl
var apiBaseUrl = Environment.GetEnvironmentVariable("API_URL");
if (string.IsNullOrEmpty(apiBaseUrl)) apiBaseUrl = builder.Configuration["localAPIURL"];
if (string.IsNullOrEmpty(apiBaseUrl)) throw new Exception("Cant get apiBaseUrl at webassembly");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
#endregion

await builder.Build().RunAsync();