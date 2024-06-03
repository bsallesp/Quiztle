﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;

namespace BrunoTheBot.Blazor.Client.APIServices.Shots
{
    public class ShotsService
    {
        private readonly HttpClient _httpClient;

        public ShotsService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<APIResponse<Shot>> CreateShotAsync(Shot shot)
        {
            var responseContent = new StringContent(JsonSerializer.Serialize(shot), Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync("api/Shots", responseContent);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                return new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Shot(),
                    Message = errorMessage
                };
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<Shot>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<Shot>> GetShotByIdAsync(Guid id)
        {
            var responseMessage = await _httpClient.GetAsync($"api/Shots/{id}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                return new APIResponse<Shot>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Shot(),
                    Message = errorMessage
                };
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<Shot>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<List<Shot>>> GetShotsByResponseIdAsync(Guid responseId)
        {
            var responseMessage = await _httpClient.GetAsync($"api/shots/{responseId}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                return new APIResponse<List<Shot>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Shot>(),
                    Message = errorMessage
                };
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<List<Shot>>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<bool>> DeleteShotAsync(Guid shotId, Guid responseId)
        {
            var url = $"api/Shots/{shotId}/{responseId}";
            Console.WriteLine($"Request URL: {url}");

            var responseMessage = await _httpClient.DeleteAsync(url);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = errorMessage
                };
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<bool>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

    }
}
