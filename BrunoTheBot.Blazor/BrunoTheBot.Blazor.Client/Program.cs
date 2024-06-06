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

builder.Services.AddTransient<AILogRepository>();

builder.Services.AddScoped<DefaultAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp
    => sp.GetRequiredService<DefaultAuthenticationStateProvider>());
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

#region POSTGRESQL CONNECTION STRING
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString)) connectionString = builder.Configuration["DevelopmentConnectionString"]!;
if (string.IsNullOrEmpty(connectionString)) throw new Exception("Cant get connections at webassembly");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
#endregion

#region BRUNOTHEBOT API URL
var brunothebotAPIURL = Environment.GetEnvironmentVariable("BRUNOTHEBOTAPIURL") ?? String.Empty;
if (string.IsNullOrEmpty(brunothebotAPIURL)) brunothebotAPIURL = builder.Configuration["DevelopmentAPIURL"] ?? string.Empty;
if (string.IsNullOrEmpty(brunothebotAPIURL)) throw new Exception("no API URLs in production, neven in development");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(brunothebotAPIURL),
    Timeout = Timeout.InfiniteTimeSpan
});
#endregion

#region PDF API URL
var pdfApiUrl = Environment.GetEnvironmentVariable("PDF_API_URL");
if (string.IsNullOrEmpty(pdfApiUrl)) pdfApiUrl = builder.Configuration["DevelopmentAPIURL"];
if (string.IsNullOrEmpty(pdfApiUrl)) throw new Exception("Cant get DevelopmentAPIURL at webassembly");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(pdfApiUrl),
    Timeout = Timeout.InfiniteTimeSpan
});
#endregion

builder.Services.AddScoped(sp => builder.HostEnvironment);

await builder.Build().RunAsync();