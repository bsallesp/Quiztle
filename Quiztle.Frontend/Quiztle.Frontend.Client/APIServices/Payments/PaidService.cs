using Quiztle.CoreBusiness.Entities.Paid;
using System.Text;
using System.Text.Json;

namespace Quiztle.Frontend.Client.APIServices.Payments
{
    public class PaidService
    {
        private readonly HttpClient _httpClient;

        public PaidService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreatePaidAsync(Paid paid)
        {
            try
            {
                var url = "api/Paid/addpaid";
                var content = new StringContent(
                    JsonSerializer.Serialize(paid),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine($"CreatePaidAsync: Failed with status {response.StatusCode}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreatePaidAsync: An exception occurred: {ex}");
                return string.Empty;
            }
        }
    }
}
