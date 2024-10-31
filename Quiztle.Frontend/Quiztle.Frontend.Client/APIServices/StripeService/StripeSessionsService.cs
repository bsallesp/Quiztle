using Stripe;
using System.Net.Http.Json;
using System.Text;
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


        public async Task<string> CreateSession(SessionStartDTO sessionStartDTO)
        {
            try
            {
                var url = "api/StripeSessions/sessions/createsession";

                var content = new StringContent(JsonSerializer.Serialize(sessionStartDTO), Encoding.UTF8, "application/json");

                var result = await _httpClient.PostAsync(url, content);

                if (result.IsSuccessStatusCode) return await result.Content.ReadAsStringAsync();

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "";
            }
        }


        public async Task<List<Stripe.Checkout.Session>> GetAllSessions()
        {
            try
            {
                var url = "api/StripeSessions/sessions/all";
                var stringResponse = await _httpClient.GetStringAsync(url);
                var result = JsonSerializer.Deserialize<List<Stripe.Checkout.Session>>(stringResponse)!;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Stripe.Checkout.Session>(); // Retorna uma lista vazia em caso de erro
            }
        }


        public async Task<bool> IsPaidSession(string sessionId, string customerId, string priceId)
        {
            try
            {
                var url = $"api/StripeSessions/sessions/ispayedsession?sessionId={Uri.EscapeDataString(sessionId)}&customerId={Uri.EscapeDataString(customerId)}&priceId={Uri.EscapeDataString(priceId)}";
                var result = await _httpClient.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    var isPaid = await result.Content.ReadFromJsonAsync<bool>();
                    return isPaid;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        public async Task<bool> IsPaidSessionByCustomer(string customerId, string priceId)
        {
            try
            {
                var url = $"api/StripeSessions/sessions/ispayedsessionbycustomer?customerId={Uri.EscapeDataString(customerId)}&priceId={Uri.EscapeDataString(priceId)}";
                var result = await _httpClient.GetAsync(url);

                if (result.IsSuccessStatusCode)
                {
                    var isPaid = await result.Content.ReadFromJsonAsync<bool>();
                    return isPaid;
                }

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
    }
}
