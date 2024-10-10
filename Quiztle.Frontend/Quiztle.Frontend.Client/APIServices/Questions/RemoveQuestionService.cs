using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using System.Text;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class RemoveQuestionService
    {
        private readonly HttpClient _httpClient;

        public RemoveQuestionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<bool>> ExecuteAsync(Guid id)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/RemoveQuestion", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<bool>(responseContent);

                if (apiResponse == false)
                {
                    return new APIResponse<bool>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = false,
                        Message = "error: question not removed: " + id
                    };
                }

                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = true,
                    Message = "deleted. " + id
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = "error: " + id + ": " + ex.Message
                };
            }
        }
    }
}