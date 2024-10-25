using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Entities.Performance;
using Quiztle.CoreBusiness.Entities.Scratch;
using Quiztle.CoreBusiness.Utils;
using Stripe;
using System.Net.Http.Json;
using System.Text.Json;

namespace Quiztle.Frontend.Client.APIServices.Performance
{
    public class AddStatsService
    {
        private readonly HttpClient _httpClient;

        public AddStatsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TestPerformance> ExecuteAsync(TestPerformance stats)
        {
            try
            {
                var url = "api/AddPerformance/";

                var stringResponse = await _httpClient.PostAsJsonAsync(url, stats);

                Console.WriteLine(stringResponse);

                return stats;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddStatsService: " + ex);

                return new();
            }
        }
    }
}