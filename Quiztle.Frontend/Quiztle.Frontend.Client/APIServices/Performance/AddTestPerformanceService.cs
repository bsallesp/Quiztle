using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Entities.Performance;
using Quiztle.CoreBusiness.Utils;
using System.Net.Http.Json;
using System.Text.Json;

namespace Quiztle.Frontend.Client.APIServices.Performance
{
    public class AddTestPerformanceService
    {
        private readonly HttpClient _httpClient;

        public AddTestPerformanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<TestPerformance>> ExecuteAsync(TestPerformance testPerformance)
        {
            try
            {
                var url = "api/AddPerformance/";

                // Logs de apoio
                Console.WriteLine($"Request URL: {url}");
                Console.WriteLine($"Base Address: {_httpClient.BaseAddress}");
                Console.WriteLine($"TestPerformance Object: {JsonSerializer.Serialize(testPerformance)}");

                var httpResponse = await _httpClient.PostAsJsonAsync(url, testPerformance);

                if (httpResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                    var testPerformanceAPIResponse = await httpResponse.Content.ReadFromJsonAsync<TestPerformance>();
                    return testPerformanceAPIResponse == null
                        ? throw new Exception("AddTestPerformanceService: testPerformanceAPIResponse is null")
                        : new APIResponse<TestPerformance>
                        {
                            Status = CustomStatusCodes.SuccessStatus,
                            Data = testPerformanceAPIResponse,
                            Message = "Test performance recorded successfully."
                        };
                }
                else
                {
                    var statusCode = (int)httpResponse.StatusCode;
                    var errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Status Code: {statusCode}");
                    Console.WriteLine($"Error Message: {errorMessage}");

                    return new APIResponse<TestPerformance>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = new TestPerformance(),
                        Message = errorMessage
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in AddTestPerformanceService: {ex.Message} | Data: {ex.Data}");

                return new APIResponse<TestPerformance>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new TestPerformance(),
                    Message = $"{ex.Message} | Data: {ex.Data}"
                };
            }
        }
    }
}
