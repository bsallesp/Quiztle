using System.Net.Http.Json;

namespace BrunoTheBot.Blazor.APIServices
{
    public class ChatGPTAPIService
    {
        private readonly HttpClient _httpClient;

        public ChatGPTAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("http://localhost:5044");
        }

        public async Task<string> GetSchoolResearch(string input, string systemProfile = "")
        {
            try
            {
                var requestData = new
                {
                    input,
                    systemProfile
                };

                var response = await _httpClient.PostAsJsonAsync("api/ChatGPT/chat", requestData);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Você pode tratar exceções aqui
                throw new HttpRequestException("Erro ao fazer a solicitação para a API ChatGPT.", ex);
            }
        }
    }
}
