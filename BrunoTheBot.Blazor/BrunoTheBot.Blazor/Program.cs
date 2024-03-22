using BrunoTheBot.Blazor.Components;
using Microsoft.EntityFrameworkCore;
using BrunoTheBot.DataContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

#region snippet1
builder.Services.AddDbContextFactory<SqliteDataContext>(opt =>
    opt.UseSqlite($"Data Source={nameof(SqliteDataContext.BrunoTheBotDb)}.db"));

Console.WriteLine($"Data Source={nameof(SqliteDataContext.BrunoTheBotDb)}.db");
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