using System.Text;
using System.Text.Json;

namespace BrunoTheBot.APIs
{
    public class ChatGPTAPI
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public ChatGPTAPI(HttpClient client)
        {
            _client = client;
            _apiKey = "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";
        }

        public async Task<string> ChatWithGPT(string input, string systemProfile = "")
        {
            if (string.IsNullOrEmpty(systemProfile))
                systemProfile = "You are a helpful assistant specialized in crafting educational content, possessing comprehensive understanding across all study-related domains. You consistently provide structured JSON responses, ensuring clarity and consistency in information delivery";

            var requestData = new
            {
                model = "gpt-3.5-turbo-0125",
                response_format = new { type = "json_object" },
                messages = new object[]
                {
                    new { role = "system", content = systemProfile },
                    new { role = "user", content = input }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                // Constrói uma mensagem de erro com mais detalhes
                string errorMessage = $"Erro ao fazer a solicitação para a API ChatGPT. Código de status: {response.StatusCode}. ";
                errorMessage += $"Motivo: {response.ReasonPhrase}. Conteúdo da resposta: {await response.Content.ReadAsStringAsync()}";
                return errorMessage;
            }
        }
    }
}
