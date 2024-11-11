using Stripe;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Quiztle.Blazor.Client.APIServices;
using Quiztle.CoreBusiness.Log;

namespace Quiztle.Frontend.Client.APIServices.StripeService
{
    public class StripeSessionsService
    {
        private readonly HttpClient _httpClient;
        private readonly CreateLogService _createLogService;
        private readonly Guid _guidLog;

        public StripeSessionsService(HttpClient httpClient, CreateLogService createLogService)
        {
            _httpClient = httpClient;
            _createLogService = new CreateLogService(_httpClient);
            _guidLog = Guid.NewGuid();
        }
        
        public async Task<string> CreateSession(SessionStartDTO sessionStartDto)
        {
            try
            {
                var logGuid = Guid.NewGuid();
                var url = "api/StripeSessions/sessions/createsession";
                var fullUrl = new Uri(_httpClient.BaseAddress, url);

                var content = new StringContent(JsonSerializer.Serialize(sessionStartDto), Encoding.UTF8, "application/json");

                var result = await _httpClient.PostAsync(fullUrl, content);
                
                await _createLogService.ExecuteAsync(new Log()
                {
                    GuidLog = logGuid,
                    Name = GetType().Name, // Dynamically gets the class name
                    Content = $"URL: {url}, " +
                              $"Headers: {string.Join(", ", _httpClient.DefaultRequestHeaders)}, " +
                              $"BaseAddress: {_httpClient.BaseAddress}, " +
                              $"Timeout: {_httpClient.Timeout}, " +
                              $"MaxResponseContentBufferSize: {_httpClient.MaxResponseContentBufferSize}"
                });

                if (result.IsSuccessStatusCode) return await result.Content.ReadAsStringAsync();

                return "";
            }
            catch (Exception ex)
            {
                await _createLogService.ExecuteAsync(new Log()
                {
                    Name = "CreateSession error",
                    Content = ex.Message,
                    GuidLog = _guidLog
                });
                
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
                return new List<Stripe.Checkout.Session>();
            }
        }
        
        public async Task<bool> IsPaidSession(string sessionId, string customerId, string priceId)
        {
            try
            {
                var url =
                    $"api/StripeSessions/sessions/ispayedsession?sessionId={Uri.EscapeDataString(sessionId)}&customerId={Uri.EscapeDataString(customerId)}&priceId={Uri.EscapeDataString(priceId)}";
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
                return false;
            }
        }


        public async Task<bool> IsPaidSessionByCustomer(string customerId, string priceId)
        {
            try
            {
                var url =
                    $"api/StripeSessions/sessions/ispayedsessionbycustomer?customerId={Uri.EscapeDataString(customerId)}&priceId={Uri.EscapeDataString(priceId)}";
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

                return result.FirstOrDefault()?.Email ?? ""; // Retorna vazio se não encontrado
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}