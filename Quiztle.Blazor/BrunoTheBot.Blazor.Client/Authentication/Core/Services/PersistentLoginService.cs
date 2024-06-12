using System.Text.Json;
using Quiztle.CoreBusiness;
using Microsoft.JSInterop;

public class PersistentLoginService
{
    private readonly IJSRuntime _jsRuntime;

    public PersistentLoginService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task WriteUserDataAsync(User userData)
    {
        try
        {
            string jsonData = JsonSerializer.Serialize(userData);

            if (Environment.GetEnvironmentVariable("RUNTIME_IDENTIFIER") == "browser-wasm")
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userData", jsonData);
            }
            else
            {
                Console.WriteLine("Storing user data in server session: " + jsonData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing user data: " + ex.Message);
        }
    }

    public async Task<User?> ReadUserDataAsync()
    {
        try
        {
            if (Environment.GetEnvironmentVariable("RUNTIME_IDENTIFIER") == "browser-wasm")
            {
                string jsonData = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userData");
                return JsonSerializer.Deserialize<User>(jsonData);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading user data: " + ex.Message);
            return null;
        }
    }
}