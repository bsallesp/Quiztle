using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Entities.Quiz;
using Quiztle.CoreBusiness.Utils;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class GetQuestionsService
    {
        private readonly HttpClient _httpClient;

        public GetQuestionsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<IEnumerable<Question>>> ExecuteAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/GetQuestions");
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<IEnumerable<Question>>(responseContent);

                if (apiResponse == null)
                {
                    return new APIResponse<IEnumerable<Question>>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = apiResponse!,
                        Message = "Error: No questions were retrieved in GetQuestionsService."
                    };
                }

                return new APIResponse<IEnumerable<Question>>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = apiResponse!,
                    Message = "Questions retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<Question>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = [],
                    Message = "Error: " + ex.Message
                };
            }
        }
    }
}
