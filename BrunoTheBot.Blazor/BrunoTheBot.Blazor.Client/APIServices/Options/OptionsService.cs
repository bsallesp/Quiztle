using System.Text.Json;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Quiz;

namespace BrunoTheBot.Blazor.Services
{
    public class OptionsService
    {
        private readonly HttpClient _httpClient;

        public OptionsService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<APIResponse<List<Option>>> GetOptionsByIdAsync(Guid optionsId)
        {
            var responseMessage = await _httpClient.GetAsync($"api/options/{optionsId}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                return new APIResponse<List<Option>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Option>(),
                    Message = errorMessage
                };
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<List<Option>>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }
}
