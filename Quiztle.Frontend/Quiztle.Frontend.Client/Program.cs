using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Quiztle.Frontend.Client;
using MudBlazor.Services;
using Quiztle.Blazor.Client.APIServices;
using Quiztle.Blazor.Client.APIServices.Tests;
using Quiztle.Frontend.Client.APIServices.Performance;
using Quiztle.Frontend.Client.Utils;
using Quiztle.Frontend.Client.APIServices.StripeService;
using Quiztle.Frontend.Client.APIServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Console.WriteLine("--------------------------------------------");
// Console.WriteLine(builder.HostEnvironment.IsDevelopment());
// Console.WriteLine(builder.HostEnvironment.IsProduction());
// Console.WriteLine("--------------------------------------------");
// Console.WriteLine($"Environment: {builder.HostEnvironment.Environment}");

#region Configura��o da API
var quiztleApiurl = builder.Configuration["ApiSettings:BaseUrl"];
if (string.IsNullOrEmpty(quiztleApiurl))
    throw new Exception("API URL is not configured in appsettings.json");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(quiztleApiurl),
    Timeout = Timeout.InfiniteTimeSpan
});
Console.WriteLine($"API Base URL Acquired in quiztle webassembly: {quiztleApiurl}");
#endregion

builder.Services.AddMudServices();
builder.Services.AddTransient<GetQuestionsService>();
builder.Services.AddTransient<GetAllScratchesService>();
builder.Services.AddTransient<GetTestByIdService>();
builder.Services.AddTransient<RemoveQuestionService>();
builder.Services.AddTransient<UpdateQuestionService>();
builder.Services.AddTransient<AddTestPerformanceService>();
builder.Services.AddTransient<GetAllTestsService>();
builder.Services.AddTransient<GetTestPerformancesByUserIdService>();
builder.Services.AddTransient<StripeCustomerService>();
builder.Services.AddTransient<StripeSessionsService>();
builder.Services.AddTransient<PaidService>();
builder.Services.AddTransient<GetUserInfos>();
builder.Services.AddTransient<CreateLogService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();

