using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Quiztle.Frontend.Client;
using MudBlazor.Services;
using Quiztle.Blazor.Client.APIServices;
using Quiztle.Blazor.Client.APIServices.Tests;
using Quiztle.Frontend.Client.APIServices.Performance;
using Quiztle.Frontend.Client.Utils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "http://localhost:5514")
        //BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:5002")
    });

builder.Services.AddMudServices();

builder.Services.AddTransient<GetQuestionsService>();
builder.Services.AddTransient<GetAllScratchesService>();
builder.Services.AddTransient<GetDraftByIdService>();
builder.Services.AddTransient<RemoveQuestionService>();
builder.Services.AddTransient<UpdateQuestionService>();
builder.Services.AddTransient<AddTestPerformanceService>();
builder.Services.AddTransient<GetTestPerformancesByUserIdService>();

builder.Services.AddTransient<GetUserInfos>();

builder.Services.AddAuthorizationCore();

//Console.WriteLine($"Environment: {builder.HostEnvironment.Environment}");

#region Configuração da API
var QuiztleAPIURL = builder.Configuration["ApiSettings:BaseUrl"];
if (string.IsNullOrEmpty(QuiztleAPIURL))
    throw new Exception("API URL is not configured in appsettings.json");

//Console.WriteLine($"API Base URL Acquired in quiztle webassembly: {QuiztleAPIURL}");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(QuiztleAPIURL),
    Timeout = Timeout.InfiniteTimeSpan
});

#endregion

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();
