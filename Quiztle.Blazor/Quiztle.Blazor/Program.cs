using Microsoft.EntityFrameworkCore;
using Quiztle.DataContext;
using Quiztle.API;
using Quiztle.DataContext.DataService.Repository.Course;
using Quiztle.Blazor.Client.APIServices;
using Quiztle.Blazor.Client.APIServices.RegularGame;
using Quiztle.Blazor.Client.Authentication.Core;
using Microsoft.AspNetCore.Components.Authorization;
using Quiztle.Blazor.Components;
using Quiztle.Blazor.Client;
using Quiztle.API.Controllers.CourseControllers.BookControllers;
using Quiztle.DataContext.Repositories;
using Quiztle.DataContext.Repositories.Quiz;
using Quiztle.Blazor.Client.APIServices.Tests;
using Quiztle.Blazor.Client.APIServices.Responses;
using Quiztle.Blazor.Client.APIServices.Shots;
using Microsoft.AspNetCore.Components.Server;
using Npgsql;
using Quiztle.API.Controllers.LLM.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.Configure<CircuitOptions>(options => { options.DetailedErrors = true; });

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient();

builder.Services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN"; });

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
var connectionString = builder.Configuration["DB_CONN"]
    ?? Environment.GetEnvironmentVariable("PROD_POSTGRES_CONNECTION_STRING")
    ?? throw new Exception("DB CONNECTION NOT FOUND");

try
{
    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Database Connection Successful");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Database Connection Failed: " + ex.Message);
}

if (string.IsNullOrEmpty(connectionString)) throw new Exception("Cant get connections at blazor server:");
Console.WriteLine("Connection adquired: " + connectionString);
#endregion

#region QuiztleAPIURL
var QuiztleAPIURL = "";
if (builder.Environment.IsProduction()) QuiztleAPIURL = Environment.GetEnvironmentVariable("PROD_API_URL") ?? string.Empty;
if (builder.Environment.IsDevelopment()) QuiztleAPIURL = builder.Configuration["DEV_API_URL"] ?? string.Empty;

if (string.IsNullOrEmpty(QuiztleAPIURL)) throw new Exception($"Blazor Server ERROR: No envirovment variable found for {nameof(QuiztleAPIURL)}");
Console.WriteLine($"{nameof(QuiztleAPIURL)} Adquired - {QuiztleAPIURL}");
Console.WriteLine($"URL gotten of: {(builder.Environment.IsProduction() ? "Production" : "Development")}");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(QuiztleAPIURL),
    Timeout = Timeout.InfiniteTimeSpan
});

// Teste de conexão à API
try
{
    using (var httpClient = new HttpClient())
    {
        var response = await httpClient.GetAsync(QuiztleAPIURL);
        Console.WriteLine(response.IsSuccessStatusCode ? "API Connection Successful" : "API Connection Failed: " + response.ReasonPhrase);
    }
}
catch (Exception ex)
{
    Console.WriteLine("API Connection Failed: " + ex.Message);
}
#endregion

builder.Services.AddScoped<CodeExtraction>();

builder.Services.AddTransient<BookRepository>();
builder.Services.AddTransient<PDFDataRepository>();

builder.Services.AddTransient<GetAllBooksController>();
builder.Services.AddTransient<ILLMChatGPTRequest, ChatGPTRequest>();

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
builder.Services.AddTransient<GetAllTestsService>();
builder.Services.AddTransient<CreateTestFromPDFService>();
builder.Services.AddTransient<CreateTestService>();
builder.Services.AddTransient<AddScratchService>();
builder.Services.AddTransient<RemoveQuestionService>();
builder.Services.AddTransient<UpdateQuestionService>();
builder.Services.AddTransient<GetQuestionsService>();
builder.Services.AddTransient<CreateUserService>();
builder.Services.AddTransient<GetUserByEmailService>();

builder.Services.AddTransient<GetAllQuestionsToRegularGame>();
builder.Services.AddTransient<CheckRenderSide>();

builder.Services.AddTransient<AILogRepository>();

builder.Services.AddScoped<DefaultAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp
    => sp.GetRequiredService<DefaultAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContextFactory<PostgreQuiztleContext>(opt =>
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


app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Quiztle.Blazor.Client._Imports).Assembly);

app.Run();