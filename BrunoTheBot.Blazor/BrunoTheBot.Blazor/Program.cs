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
using BrunoTheBot.Blazor.Client.APIServices.Responses;
using BrunoTheBot.Blazor.Client.APIServices.Shots;
using Microsoft.AspNetCore.Components.Server;

Console.WriteLine("testing here");

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.Configure<CircuitOptions>(options =>
{
    options.DetailedErrors = true;
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

#region Postgresql Connection
var connectionString = "Host=brunothebot-postgres;Database=BrunoTheBotDB;Username=brunothebotuser;Password=@pyramid2050!";
//if (string.IsNullOrEmpty(connectionString)) connectionString = Environment.GetEnvironmentVariable("PROD_POSTGRES_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString)) throw new Exception("Cant get connections at blazor server:");
Console.WriteLine("Connection adquired: " + connectionString);
#endregion

#region brunothebotAPIURL
var brunothebotAPIURL = "";
if (builder.Environment.IsProduction()) brunothebotAPIURL = Environment.GetEnvironmentVariable("PROD_API_URL") ?? string.Empty;
if (builder.Environment.IsDevelopment()) brunothebotAPIURL = builder.Configuration["DEV_API_URL"] ?? string.Empty;

if (string.IsNullOrEmpty(brunothebotAPIURL)) throw new Exception($"Blazor Server ERROR: No envirovment variable found for {nameof(brunothebotAPIURL)}");
else Console.WriteLine($"{nameof(brunothebotAPIURL)} Adquired - {brunothebotAPIURL}");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(brunothebotAPIURL),
    Timeout = Timeout.InfiniteTimeSpan
});
#endregion

#region Flask API URL
var pdfApiUrl = "";
if (builder.Environment.IsProduction()) pdfApiUrl = Environment.GetEnvironmentVariable("PROD_FLASK_API_URL") ?? string.Empty;
if (builder.Environment.IsDevelopment()) pdfApiUrl = builder.Configuration["DEV_FLASK_API_URL"] ?? string.Empty;

if (string.IsNullOrEmpty(brunothebotAPIURL)) throw new Exception($"Blazor Server ERROR: No envirovment variable found for {nameof(pdfApiUrl)}");
else Console.WriteLine($"{nameof(pdfApiUrl)} in Blazor server Adquired - {pdfApiUrl}");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(pdfApiUrl),
    Timeout = Timeout.InfiniteTimeSpan
});
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
builder.Services.AddTransient<CreateTestService>();
builder.Services.AddTransient<RemoveTestService>();
builder.Services.AddTransient<ResponsesService>();
builder.Services.AddTransient<ShotsService>();
builder.Services.AddTransient<UploadFileService>();
builder.Services.AddTransient<UploadedFilesListService>();
builder.Services.AddTransient<PDFToDataFromStreamService>();

builder.Services.AddTransient<GetAllQuestionsToRegularGame>();
builder.Services.AddTransient<CheckRenderSide>();

builder.Services.AddTransient<AILogRepository>();

builder.Services.AddScoped<DefaultAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp
    => sp.GetRequiredService<DefaultAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContextFactory<PostgreBrunoTheBotContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BrunoTheBot.Blazor.Client._Imports).Assembly);

app.Run();