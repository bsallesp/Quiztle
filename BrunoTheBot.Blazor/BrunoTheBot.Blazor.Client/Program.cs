using BrunoTheBot.Blazor.Client;
using BrunoTheBot.Blazor.Client.APIServices;

using BrunoTheBot.Blazor.Client.APIServices.RegularGame;
using BrunoTheBot.Blazor.Client.APIServices.Responses;
using BrunoTheBot.Blazor.Client.APIServices.Shots;
using BrunoTheBot.Blazor.Client.APIServices.Tests;
using BrunoTheBot.Blazor.Client.Authentication.Core;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using BrunoTheBot.DataContext.Repositories;
using BrunoTheBot.DataContext.Repositories.Quiz;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<CodeExtraction>();

builder.Services.AddTransient<BookRepository>();
builder.Services.AddTransient<PDFDataRepository>();

builder.Services.AddTransient<GetAllBooksService>();
builder.Services.AddTransient<GetAllPDFDataService>();
builder.Services.AddTransient<GetAllTestsByPDFDataIdService>();
builder.Services.AddTransient<GetTestByIdService>();

builder.Services.AddTransient<RetrieveBookByIdService>();
builder.Services.AddTransient<GetAllQuestionsToRegularGame>();
builder.Services.AddTransient<CheckRenderSide>();
builder.Services.AddTransient<CreateBookTaskService>();
builder.Services.AddTransient<CreateTestService>();
builder.Services.AddTransient<RemoveTestService>();
builder.Services.AddTransient<ResponsesService>();
builder.Services.AddTransient<ShotsService>();
builder.Services.AddTransient<UploadFileService>();
builder.Services.AddTransient<UploadedFilesListService>();
builder.Services.AddTransient<PDFToDataFromStreamService>();

builder.Services.AddTransient<AILogRepository>();

builder.Services.AddScoped<DefaultAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp
    => sp.GetRequiredService<DefaultAuthenticationStateProvider>());
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

#region Postgresql Connection
var connectionString = "";
//var connectionString = Environment.GetEnvironmentVariable("PROD_POSTGRES_CONNECTION_STRING");
connectionString = "Host=brunothebot-postgres;Database=BrunoTheBotDB;Username=brunothebotuser;Password=@pyramid2050!";
if (string.IsNullOrEmpty(connectionString)) throw new Exception("Cant get connections at webassembly");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
#endregion

#region brunothebotAPIURL
var brunothebotAPIURL = Environment.GetEnvironmentVariable("PROD_API_URL") ?? String.Empty;
if (!string.IsNullOrEmpty(brunothebotAPIURL)) Console.WriteLine($"Production Envrirovment Variable Adquired: {brunothebotAPIURL} - {nameof(brunothebotAPIURL)}");
else if (string.IsNullOrEmpty(brunothebotAPIURL)) brunothebotAPIURL = builder.Configuration["DEV_API_URL"] ?? string.Empty;
if (!string.IsNullOrEmpty(brunothebotAPIURL)) Console.WriteLine($"Development Envrirovment Variable Adquired: {brunothebotAPIURL} - {nameof(brunothebotAPIURL)}");
if (string.IsNullOrEmpty(brunothebotAPIURL)) throw new Exception("no API URLs in production, neven in development");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(brunothebotAPIURL),
    Timeout = Timeout.InfiniteTimeSpan
});
#endregion

//#region Flask API URL
//var pdfApiUrl = Environment.GetEnvironmentVariable("PROD_FLASK_API_URL") ?? String.Empty;
//if (!string.IsNullOrEmpty(pdfApiUrl)) Console.WriteLine($"Production Envrirovment Variable Adquired: {pdfApiUrl} - {nameof(pdfApiUrl)}");
//else if (string.IsNullOrEmpty(pdfApiUrl)) pdfApiUrl = builder.Configuration["DEV_FLASK_API_URL"] ?? string.Empty;
//if (!string.IsNullOrEmpty(pdfApiUrl)) Console.WriteLine($"Development Envrirovment Variable Adquired: {pdfApiUrl} - {nameof(pdfApiUrl)}");
//if (string.IsNullOrEmpty(pdfApiUrl)) throw new Exception("no API URLs in production, neven in development");

//builder.Services.AddScoped(sp => new HttpClient
//{
//    BaseAddress = new Uri(pdfApiUrl),
//    Timeout = Timeout.InfiniteTimeSpan
//});
//#endregion

builder.Services.AddScoped(sp => builder.HostEnvironment);

await builder.Build().RunAsync();