using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System.Net.Http.Json;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices.Tests
{
    public class CreateTestService
    {
        private readonly HttpClient _httpClient;

        public CreateTestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<Test>> ExecuteAsync(Test test)
        {
            try
            {
                var url = "api/CreateTest/";
                var httpResponse = await _httpClient.PostAsJsonAsync(url, test);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var testsAPIResponse = await httpResponse.Content.ReadFromJsonAsync<Test>();
                    return testsAPIResponse == null
                        ? throw new Exception("CreateTestService: testsAPIResponse is null")
                        : new APIResponse<Test>
                        {
                            Status = CustomStatusCodes.SuccessStatus,
                            Data = testsAPIResponse,
                            Message = "Total questions: " + testsAPIResponse.Questions.Count.ToString()
                        };
                }
                else
                {
                    var statusCode = (int)httpResponse.StatusCode;
                    var errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    return new APIResponse<Test>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = new Test(),
                        Message = errorMessage
                    };
                }

            }
            catch (Exception ex)
            {
                return new APIResponse<Test>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Test(),
                    Message = ex.Message + " " + ex.Data.ToString()
                };
            }
        }
    }
}
