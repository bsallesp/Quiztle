using BrunoTheBot.CoreBusiness.Log;
using BrunoTheBot.DataContext.Repositories;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BrunoTheBot.API
{
    public class ChatGPTRequest : IChatGPTRequest, IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly AILogRepository _logRepository;
        private string _sessionId; // Adicionado para gerenciar a sessão

        public ChatGPTRequest(HttpClient client, AILogRepository aILogRepository, string sessionId = null)
        {
            _client = client;
            _apiKey = "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";
            _logRepository = aILogRepository;
            _sessionId = sessionId ?? Guid.NewGuid().ToString(); // Gera um novo ID de sessão se não fornecido
        }

        public async Task<string> ExecuteAsync(string input, string systemProfile = "")
        {
            try
            {
                if (string.IsNullOrEmpty(systemProfile))
                    systemProfile = "You are a helpful assistant specialized in crafting educational content, possessing comprehensive understanding across all study-related domains. You consistently provide structured JSON responses, ensuring clarity and consistency in information delivery";

                var requestData = new
                {
                    model = "gpt-3.5-turbo-0125",
                    session_id = _sessionId, // Inclui session_id na solicitação
                    response_format = new { type = "json_object" },
                    messages = new object[]
                    {
                        new { role = "system", content = systemProfile },
                        new { role = "user", content = input }
                    }
                };

                var jsonRequest = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                if (!_client.DefaultRequestHeaders.Any())
                    _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                await _logRepository.CreateAILogAsync(new AILog
                {
                    JSON = jsonRequest,
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
                    string errorMessage = $"Erro ao fazer a solicitação para a API ChatGPT. Código de status: {response.StatusCode}. ";
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

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
