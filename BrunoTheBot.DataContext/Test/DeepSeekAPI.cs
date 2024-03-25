using System.Text;
using Newtonsoft.Json;

namespace BrunoTheBot.APIs
{
    public class DeepSeekAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "sk-62c786ca9d7f49ffa383ac9815799642"; // Substitua pelo seu API key do DeepSeek

        public DeepSeekAPI()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GenerateTextAsync(string inputText)
        {
            try
            {
                var data = new
                {
                    model = "deepseek-chat",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful assistant." },
                        new { role = "user", content = inputText }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

                var url = "https://api.deepseek.com/v1/chat/completions";
                var response = await _httpClient.PostAsync(url, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return responseContent;
                }
                else
                {
                    throw new Exception($"Erro ao gerar texto: {(int)response.StatusCode} - {response.ReasonPhrase}. Detalhes: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar texto", ex);
            }
        }
    }
}

//DEEPSEEK
//sk-62c786ca9d7f49ffa383ac9815799642
