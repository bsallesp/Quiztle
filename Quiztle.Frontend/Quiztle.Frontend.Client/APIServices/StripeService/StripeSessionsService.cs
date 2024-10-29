using Stripe;
using System.Net.Http.Json;
using System.Text.Json;

namespace Quiztle.Frontend.Client.APIServices.StripeService
{
    public class StripeSessionsService
    {
        private readonly HttpClient _httpClient;

        public StripeSessionsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateSession(
            string priceId,
            string customerId,
            string customerEmail)
        {
            try
            {
                Console.WriteLine(priceId);
                Console.WriteLine(customerId);
                Console.WriteLine(customerEmail);

                var url = $"api/StripeSessions/sessions/createsession?priceID={priceId}&customerId={customerId}&customerEmail={customerEmail}";

                var result = await _httpClient.GetAsync(url);

                if (result.IsSuccessStatusCode) return await result.Content.ReadAsStringAsync();

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "";
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
