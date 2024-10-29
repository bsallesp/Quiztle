using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quiztle.Frontend.Components;
using Quiztle.Frontend.Components.Account;
using Quiztle.Frontend.Data;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Server;
using Stripe;
using Npgsql;
using Quiztle.Blazor.Client.APIServices;
using Quiztle.Blazor.Client.APIServices.Tests;
using Quiztle.Frontend.Client.APIServices.Performance;
using Quiztle.Frontend.Client.Utils;
using Quiztle.Frontend.Client.APIServices.StripeService;

var builder = WebApplication.CreateBuilder(args);

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Program");

logger.LogError("Fail test.");

builder.Services.Configure<CircuitOptions>(options => { options.DetailedErrors = true; });

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddTransient<GetUserInfos>();

builder.Services.AddTransient<GetQuestionsService>();
builder.Services.AddTransient<GetAllScratchesService>();
builder.Services.AddTransient<GetDraftByIdService>();
builder.Services.AddTransient<RemoveQuestionService>();
builder.Services.AddTransient<UpdateQuestionService>();
builder.Services.AddTransient<AddTestPerformanceService>();
builder.Services.AddTransient<GetTestPerformancesByUserIdService>();

builder.Services.AddTransient<StripeCustomerService>();
builder.Services.AddTransient<StripeSessionsService>();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("PROD_POSTGRES_CONNECTION_STRING")
    ?? throw new InvalidOperationException("DB CONNECTION NOT FOUND");

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
Console.WriteLine($"{nameof(QuiztleAPIURL)}: URL Adquired - {QuiztleAPIURL}");
Console.WriteLine($"URL gotten of: {(builder.Environment.IsProduction() ? "Production" : "Development")}");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(QuiztleAPIURL),
    Timeout = Timeout.InfiniteTimeSpan
});

try
{
    Console.WriteLine($"Testing API URL...{QuiztleAPIURL}");
    using (var httpClient = new HttpClient())
    {
        var response = await httpClient.PostAsync(QuiztleAPIURL + "api/TestConn/TestConnection", null);
        Console.WriteLine(response.IsSuccessStatusCode ? "API Connection Successful" : "API Connection Failed: " + response.ReasonPhrase);
    }
}
catch (Exception ex)
{
    Console.WriteLine("API Connection Failed: " + ex.Message);
}
#endregion

#region Identity / Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddGoogle(options =>
{
    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthNSection["ClientId"] ?? throw new Exception("Google ClientId wasn't found in program.cs");
    options.ClientSecret = googleAuthNSection["ClientSecret"] ?? throw new Exception("Google ClientId wasn't found in program.cs");
    options.CallbackPath = "/signin-google";
}).AddIdentityCookies();

#endregion

#region DataContext

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#endregion

#region AddIdentityCore
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
#endregion

//Real
//StripeConfiguration.ApiKey = "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";
//Test
StripeConfiguration.ApiKey = "sk_test_51QAsiRLKiSsrfvcHtmFizIraAZoHURLCDugHPuq7FQ5bN1vv3rGVpaiclj1cYdiOYJtHyUJrB7PImAHDcbPUlqFf00Fvu61m3n";

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Quiztle.Frontend.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
