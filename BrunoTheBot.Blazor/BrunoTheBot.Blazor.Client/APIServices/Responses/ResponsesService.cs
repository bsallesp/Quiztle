using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;

namespace BrunoTheBot.Blazor.Client.APIServices.Responses
{
    public class ResponsesService
    {
        private readonly HttpClient _httpClient;

        public ResponsesService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<APIResponse<Response>> CreateResponseAsync(Response response)
        {
            var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, "application/json");
            var responseMessage = await _httpClient.PostAsync("api/Responses", responseContent);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = errorMessage
                };
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<Response>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<Response>> GetResponseByIdAsync(Guid id)
        {
            var responseMessage = await _httpClient.GetAsync($"api/Responses/{id}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = errorMessage
                };
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<Response>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<APIResponse<Response>> GetUnfinalizedResponseByTestIdAsync(Guid testId)
        {
            var responseMessage = await _httpClient.GetAsync($"api/Responses/notfinalized/{testId}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();
                return new APIResponse<Response>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Response(),
                    Message = errorMessage
                };
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<APIResponse<Response>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }
}
