using System.Text;
using System.Text.Json;

namespace Quiztle.API
{
    public class OllamaRequest : IChatGPTRequest
    {
        private readonly HttpClient _client;
        private readonly string _ngrokEndpoint = "https://6d9f-2600-1700-6a32-e650-5818-9fb5-102c-346b.ngrok-free.app/api/generate";

        public OllamaRequest(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> ExecuteAsync(string input, string systemProfile = "")
        {
            try
            {
                if (string.IsNullOrEmpty(systemProfile))
                    systemProfile = "You are a helpful assistant specialized in crafting educational content, possessing comprehensive understanding across all study-related domains. You consistently provide structured JSON responses, ensuring clarity and consistency in information delivery";

                var requestData = new
                {
                    model = "llama3.1",
                    stream = false,
                    format =  "json",
                    prompt = input
                };

                var jsonRequest = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_ngrokEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    JsonDocument document = JsonDocument.Parse(jsonResponse);
                    JsonElement root = document.RootElement;
                    string responseJsonString = root.GetProperty("response").GetString() ?? "";

                    Console.WriteLine("----------OLLAMA RESPONSE: " + responseJsonString);
                    return jsonResponse;
                }
                else
                {
                    string errorMessage = $"Erro ao fazer a solicitação para o túnel Ngrok. Código de status: {response.StatusCode}. ";
                    errorMessage += $"Motivo: {response.ReasonPhrase}. Conteúdo da resposta: {await response.Content.ReadAsStringAsync()}";
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";
                if (ex.InnerException != null)
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                errorMessage += $" StackTrace: {ex.StackTrace}";
                throw new Exception(errorMessage);
            }
        }
    }
}
