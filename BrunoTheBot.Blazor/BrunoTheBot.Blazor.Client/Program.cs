using BrunoTheBot.APIs;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient<DeepSeekAPI>();

await builder.Build().RunAsync();
