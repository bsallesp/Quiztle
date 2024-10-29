using Stripe;
using Stripe.Reporting;
using System.Text.Json;

namespace Quiztle.Frontend.Client.APIServices.StripeService
{
    public class StripeCustomerService
    {
        private readonly HttpClient _httpClient;

        public StripeCustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Customer>> SearchCustomerByName(string name)
        {
            try
            {
                var url = $"api/StripeCustomer/search/name?name={name}";
                var stringResponse = await _httpClient.GetStringAsync(url);

                var result = JsonSerializer.Deserialize<List<Customer>>(stringResponse)!;

                return result;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return [];
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
