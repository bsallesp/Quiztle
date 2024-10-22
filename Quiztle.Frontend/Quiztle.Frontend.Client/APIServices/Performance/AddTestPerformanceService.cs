using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Entities.Performance;
using Quiztle.CoreBusiness.Entities.Scratch;
using Quiztle.CoreBusiness.Utils;
using Stripe;
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

        public async Task<TestPerformance> ExecuteAsync(TestPerformance testPerformance)
        {
            try
            {
                var url = "api/AddPerformance/";

                var stringResponse = await _httpClient.PostAsJsonAsync(url, testPerformance);

                Console.WriteLine(stringResponse);

                return testPerformance;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddTestPerformanceService: " + ex);

                return new();
            }
        }
    }
}