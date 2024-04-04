using BrunoTheBot.CoreBusiness.Log;
using System.Net.Http.Json;

namespace BrunoTheBot.Blazor.APIServices
{
    public class AILogService
    {
        private readonly HttpClient _httpClient;

        public AILogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5044"); // Substitua pela URL da sua API
        }

        public async Task<IEnumerable<AILog>> GetAILogsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<AILog>>("api/AILogs");
            return response ?? Enumerable.Empty<AILog>();
        }

        public async Task<AILog> GetAILogAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<AILog>($"api/AILogs/{id}");
            if (response == null) throw new Exception();
            return response;
        }

        public async Task<AILog> CreateAILogAsync(AILog aILog)
        {
            var response = await _httpClient.PostAsJsonAsync("api/AILogs", aILog);
            Console.WriteLine("JSON AT GetAILogsAsync BLAZOR: " + aILog.JSON);
            response.EnsureSuccessStatusCode();
            if (response == null) throw new Exception();
            return await response.Content.ReadFromJsonAsync<AILog>() ?? throw new Exception();
        }

        public async Task UpdateAILogAsync(int id, AILog aILog)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/AILogs/{id}", aILog);
            if (response == null) throw new Exception();
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAILogAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/AILogs/{id}");
            if (response == null) throw new Exception();
            response.EnsureSuccessStatusCode();
        }
    }
}
