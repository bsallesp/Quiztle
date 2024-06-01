using System.Text;
using System.Text.Json;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.CoreBusiness.APIEntities;

namespace BrunoTheBot.Blazor.Client.APIServices.Responses
{
    public class ResponsesService
    {
        private readonly HttpClient _httpClient;

        public ResponsesService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<APIResponse<List<Response>>> GetAllResponsesAsync()
        {
            var response = await _httpClient.GetAsync("api/Responses");
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<List<Response>>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<Response>> GetResponseByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Responses/{id}");
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<Response>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<Response>> CreateResponseAsync(Response response)
        {
            var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync("api/Responses", responseContent);
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<Response>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<bool>> UpdateResponseAsync(Guid id, Response response)
        {
            var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PutAsync($"api/Responses/{id}", responseContent);
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<bool>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<bool>> DeleteResponseAsync(Guid id)
        {
            var responseMessage = await _httpClient.DeleteAsync($"api/Responses/{id}");
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<bool>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<bool>> ExistsByTestIdAsync(Guid testId)
        {
            var response = await _httpClient.GetAsync($"api/Responses/existsByTestId/{testId}");
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<bool>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }
}
