using Quiztle.CoreBusiness.Entities.Paid;
using System.Net.Http.Json;

namespace Quiztle.Frontend.Client.APIServices
{
    public class PaidService
    {
        private readonly HttpClient _httpClient;

        public PaidService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsPaidService(Paid paid)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/paid/ispaid", paid);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IsPaidService: {ex.Message}");
                return false;
            }
        }
    }
}
