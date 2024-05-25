using Microsoft.EntityFrameworkCore;
using BrunoTheBot.DataContext;
using BrunoTheBot.API;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using BrunoTheBot.Blazor.Client.APIServices;
using BrunoTheBot.Blazor.Client.APIServices.RegularGame;
using BrunoTheBot.Blazor.Client.Authentication.Core;
using Microsoft.AspNetCore.Components.Authorization;
using BrunoTheBot.Blazor.Components;
using BrunoTheBot.Blazor.Client;
using BrunoTheBot.API.Controllers.CourseControllers.BookControllers;
using BrunoTheBot.DataContext.Repositories;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.Blazor.Client.APIServices.Tests;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient();

#region apiBaseUrl
var apiBaseUrl = Environment.GetEnvironmentVariable("API_URL");
if (string.IsNullOrEmpty(apiBaseUrl)) apiBaseUrl = builder.Configuration["localAPIURL"];
if (string.IsNullOrEmpty(apiBaseUrl)) throw new Exception();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
#endregion


builder.Services.AddScoped<CodeExtraction>();

builder.Services.AddTransient<BookRepository>();
builder.Services.AddTransient<PDFDataRepository>();

builder.Services.AddTransient<GetAllBooksController>();
builder.Services.AddTransient<IChatGPTRequest, ChatGPTRequest>();

builder.Services.AddTransient<GetAllBooksService>();
builder.Services.AddTransient<RetrieveBookByIdService>();
builder.Services.AddTransient<GetAllPDFDataService>();
builder.Services.AddTransient<CreateBookTaskService>();
builder.Services.AddTransient<GetAllTestsByPDFDataIdService>();
builder.Services.AddTransient<GetTestByIdService>();

builder.Services.AddTransient<GetAllQuestionsToRegularGame>();
builder.Services.AddTransient<CheckRenderSide>();

builder.Services.AddTransient<AILogRepository>();

builder.Services.AddScoped<DefaultAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp
    => sp.GetRequiredService<DefaultAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

#region connectionString
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString)) connectionString = builder.Configuration["ConnectionString"]!;
if (string.IsNullOrEmpty(connectionString)) throw new Exception("Cant get connections at webassembly");
#endregion

builder.Services.AddDbContextFactory<PostgreBrunoTheBotContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

var app = builder.Build();

//await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
//var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<PostgreBrunoTheBotContext>>();
//await DatabaseUtility.EnsureDbCreatedAndSeedWithCountOfAsync(options, 500);

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BrunoTheBot.Blazor.Client._Imports).Assembly);

app.Run();