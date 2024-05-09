using BrunoTheBot.CoreBusiness.Log;
using BrunoTheBot.DataContext.Repositories;
using System.Text;
using System.Text.Json;

namespace BrunoTheBot.API
{
    public class ChatGPTRequest : IChatGPTRequest, IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly AILogRepository _logRepository;

        public ChatGPTRequest(HttpClient client, AILogRepository aILogRepository)
        {
            _client = client;
            _apiKey = "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";
            _logRepository = aILogRepository;
        }

        public async Task<string> ChatWithGPT(string input, string systemProfile = "")
        {
            try
            {
                if (string.IsNullOrEmpty(systemProfile))
                    systemProfile = "You are a helpful assistant specialized in crafting educational content, possessing comprehensive understanding across all study-related domains. You consistently provide structured JSON responses, ensuring clarity and consistency in information delivery";

                var requestData = new
                {
                    model = "gpt-3.5-turbo-0125",
                    //model = "gpt-4 Turbo",
                    response_format = new { type = "json_object" },
                    messages = new object[]
                    {
                    new { role = "system", content = systemProfile },
                    new { role = "user", content = input }
                    }
                };

                var jsonRequest = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                if (!_client.DefaultRequestHeaders.Any()) _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                await _logRepository.CreateAILogAsync(new AILog
                {
                    JSON = content.ToString() ?? "The content is null",
                    Name = "ChapGPTRequest"

                }); 

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
            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
        }

        // Implementação do método Dispose para liberar recursos
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
