using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices.Tests
{
    public class GetAllTests
    {
        private readonly HttpClient _httpClient;

        public GetAllTests(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<List<Test>>> ExecuteAsync()
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetAllTests/");
                List<Test> dataTemp = JsonSerializer.Deserialize<List<Test>>(stringResponse)!;

                APIResponse<List<Test>> apiResult = new APIResponse<List<Test>> { Data = dataTemp };

                return apiResult;
            }
            catch
            {
                return new APIResponse<List<Test>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Test>()
                };
            }
        }
    }
}