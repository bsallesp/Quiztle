using Stripe;
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

        public async Task<string> CreateCustomer(string name, string email)
        {
            try
            {
                var url = $"api/StripeCustomer/customer/create?name={name}?email={email}";
                var response = await _httpClient.GetStringAsync(url);

                return response;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "";
            }
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

        public async Task<string> SearchCustomerIdByEmail(string email)
        {
            try
            {
                var url = $"api/StripeCustomer/search/customeridbyemail?email={email}";
                var stringResponse = await _httpClient.GetStringAsync(url);
                Console.WriteLine(stringResponse);
                return stringResponse;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }
    }
}
