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
                var url = $"api/StripeCustomer/customer/create?name={Uri.EscapeDataString(name)}&email={Uri.EscapeDataString(email)}";
                var response = await _httpClient.PostAsync(url, null); // Usando POST para criar cliente

                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    return stringResponse;
                }

                return "";
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
                var url = $"api/StripeCustomer/search/name?name={Uri.EscapeDataString(name)}";
                var stringResponse = await _httpClient.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<List<Customer>>(stringResponse)!;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Customer>(); // Retorna uma lista vazia em caso de erro
            }
        }

        public async Task<string> SearchCustomerByEmail(string email)
        {
            try
            {
                var url = $"api/StripeCustomer/search/email?email={Uri.EscapeDataString(email)}";
                var stringResponse = await _httpClient.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<List<Customer>>(stringResponse)!;

                Console.WriteLine(result);
                return result.FirstOrDefault()?.Email ?? ""; // Retorna vazio se não encontrado
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
                var url = $"api/StripeCustomer/search/customeridbyemail?email={Uri.EscapeDataString(email)}";
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

        public async Task<List<object>> ListAllCustomers()
        {
            try
            {
                var url = $"api/StripeCustomer/customer/listall";
                var stringResponse = await _httpClient.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<List<object>>(stringResponse)!;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<object>(); // Retorna uma lista vazia em caso de erro
            }
        }
    }
}
