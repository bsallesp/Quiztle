using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices.Tests
{
    public class GetDraftByIdService
    {
        private readonly HttpClient _httpClient;

        public GetDraftByIdService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<Test>> ExecuteAsync(Guid id)
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetTestById/" + id);
                Test testResponse = JsonSerializer.Deserialize<Test>(stringResponse)!;

                return new APIResponse<Test>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = testResponse,
                    Message = "Total questions: " + testResponse.Questions.Count.ToString()
                };
                
            }
            catch
            {
                return new APIResponse<Test>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Test()
                };
            }
        }
    }
}