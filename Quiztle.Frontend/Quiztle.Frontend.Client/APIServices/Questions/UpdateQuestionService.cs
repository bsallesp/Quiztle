using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System.Text;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class UpdateQuestionService
    {
        private readonly HttpClient _httpClient;

        public UpdateQuestionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<bool>> ExecuteAsync(Guid id, Question updatedQuestion)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(updatedQuestion), Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/UpdateQuestion/{id}", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<bool>>(responseContent);

                if (apiResponse == null || apiResponse.Data == false)
                {
                    return new APIResponse<bool>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = false,
                        Message = "error: question not updated: " + id
                    };
                }

                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = true,
                    Message = "updated. " + id
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
