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
using System.Security.Policy;
using Quiztle.Blazor.Client.APIServices.Tests;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddTransient<GetQuestionsService>();
builder.Services.AddTransient<GetAllScratchesService>();
builder.Services.AddTransient<GetTestByIdService>();
builder.Services.AddTransient<RemoveQuestionService>();
builder.Services.AddTransient<UpdateQuestionService>();

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

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

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

StripeConfiguration.ApiKey = "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";

var app = builder.Build();

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
