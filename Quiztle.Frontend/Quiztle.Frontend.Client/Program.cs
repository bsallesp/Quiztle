using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Quiztle.Frontend.Client;
using MudBlazor.Services;
using Quiztle.Blazor.Client.APIServices;
using Quiztle.Blazor.Client.APIServices.Tests;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:5002")
    });

builder.Services.AddMudServices();

builder.Services.AddTransient<GetQuestionsService>();
builder.Services.AddTransient<GetAllScratchesService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();
