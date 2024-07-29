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

                Console.WriteLine(url);
                Console.WriteLine($"Base Address: {_httpClient.BaseAddress}");
                Console.WriteLine($"Test Object: {JsonSerializer.Serialize(test)}");

                var httpResponse = await _httpClient.PostAsJsonAsync(url, test);

                if (httpResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Sucess");
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
                    Console.WriteLine($"Error Status Code: {statusCode}");
                    Console.WriteLine($"Error Message: {errorMessage}");
                    Console.WriteLine($"Error Response Content: {errorMessage}");


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
                Console.WriteLine(ex.Message + " ERROR IN CreateTestService... : " + ex.Data.ToString());

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
