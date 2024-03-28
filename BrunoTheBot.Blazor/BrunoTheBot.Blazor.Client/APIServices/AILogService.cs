﻿using BrunoTheBot.CoreBusiness;
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
            return response;
        }

        public async Task<AILog> CreateAILogAsync(AILog aILog)
        {
            var response = await _httpClient.PostAsJsonAsync("api/AILogs", aILog);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AILog>();
        }

        public async Task UpdateAILogAsync(int id, AILog aILog)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/AILogs/{id}", aILog);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAILogAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/AILogs/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
