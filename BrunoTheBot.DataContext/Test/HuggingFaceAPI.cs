using System.Text;
using Newtonsoft.Json;

namespace BrunoTheBot.APIs
{
    public class HuggingFaceAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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";

        public HuggingFaceAPI()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GenerateTextAsync(string inputText)
        {
            var data = new
            {
                inputs = inputText
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://api-inference.huggingface.co/models/gpt2", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                // Processar a resposta e extrair o texto gerado
                // ...
                return responseContent; // Retornar o texto gerado
            }

            return "Erro ao gerar texto";
        }
    }

}
