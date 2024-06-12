using System.Text.Json;
using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.Blazor.Services
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
