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

// Configuração da API
var brunothebotAPIURL = builder.Configuration["ApiSettings:BaseUrl"];
if (string.IsNullOrEmpty(brunothebotAPIURL))
    throw new Exception("API URL is not configured in appsettings.json");

Console.WriteLine($"API Base URL Acquired: {brunothebotAPIURL}");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(brunothebotAPIURL),
    Timeout = Timeout.InfiniteTimeSpan
});

builder.Services.AddScoped(sp => builder.HostEnvironment);

await builder.Build().RunAsync();
