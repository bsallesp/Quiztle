using Quiztle.CoreBusiness.Entities.Performance;
using System.Text.Json;

namespace Quiztle.Frontend.Client.APIServices.Performance
{
    public class GetStatsByUserIdService
    {
        private readonly HttpClient _httpClient;

        public GetStatsByUserIdService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TestPerformance>> ExecuteAsync(Guid userId)
        {
            try
            {
                Console.WriteLine(_httpClient.BaseAddress);

                var url = "api/GetTestPerformancesByUserId/";

                var endpoint = url + userId.ToString();

                Console.WriteLine(endpoint);

                var stringResponse = await _httpClient.GetStringAsync(endpoint);

                var performances = JsonSerializer.Deserialize<IEnumerable<TestPerformance>>(stringResponse);

                return performances!;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddStatsService: " + ex);

                return [];
            }
        }
    }
}