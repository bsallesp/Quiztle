using Quiztle.CoreBusiness.Entities.Paid;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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


        public async Task<IEnumerable<Paid>?> GetPaidByEmailService(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/paid/bypaidemail?email={email}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<Paid>>();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPaidByEmailService: {ex.Message}");
                return null;
            }
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
