using Stripe;
using System.Net.Http.Json;
using System.Text.Json;

namespace Quiztle.Frontend.Client.APIServices.StripeService
{
    public class StripePaymentService
    {
        private readonly HttpClient _httpClient;

        public StripePaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateSession(string priceID)
        {
            try
            {
                var url = "api/StripeSessions/sessions/createsession?priceID=" + priceID;
                var result = await _httpClient.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine(await result.Content.ReadAsStringAsync());
                    return true;
                }

                Console.WriteLine($"Error: {result.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        public async Task<string> SearchCustomerByEmail(string email)
        {
            try
            {
                var url = $"api/StripeCustomer/search/email?email={email}";
                var stringResponse = await _httpClient.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<List<Customer>>(stringResponse)!;


                Console.WriteLine(result);
                return result.FirstOrDefault()!.Email;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }
    }
}
