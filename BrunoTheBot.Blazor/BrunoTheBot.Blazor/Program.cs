using Microsoft.EntityFrameworkCore;
using BrunoTheBot.DataContext;
using BrunoTheBot.APIs;
using BrunoTheBot.Blazor.APIServices;
using BrunoTheBot.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient();

builder.Services.AddTransient<SchoolRepository>();
builder.Services.AddTransient<HuggingFaceAPI>();
builder.Services.AddTransient<DeepSeekAPI>();
builder.Services.AddTransient<AILogService>();
builder.Services.AddScoped<ChatGPTAPI>();
builder.Services.AddScoped<ChatGPTAPIService>();

#region snippet1
builder.Services.AddDbContextFactory<SqliteDataContext>(opt =>
{
    string connectionString = ConnectionStrings.DevelopmentConnectionString;

    var env = builder.Environment;
    if (env.IsProduction()) connectionString = ConnectionStrings.ProductionConnectionString;

    Console.WriteLine(connectionString);
    opt.UseSqlite(connectionString);
});
#endregion

var app = builder.Build();

await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<SqliteDataContext>>();
await DatabaseUtility.EnsureDbCreatedAndSeedWithCountOfAsync(options, 500);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BrunoTheBot.Blazor.Client._Imports).Assembly);

app.Run();