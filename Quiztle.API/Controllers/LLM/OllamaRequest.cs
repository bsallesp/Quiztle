using System.Text;
using System.Text.Json;
using Quiztle.API.Controllers.LLM.Interfaces;

namespace Quiztle.API.Controllers.LLM
{
    public class OllamaRequest : ILLMRequest
    {
        private readonly HttpClient _client;
        private readonly IEndpointProvider _endpointProvider;

        public OllamaRequest(HttpClient client, IEndpointProvider endpointProvider)
        {
            _client = client;
            _endpointProvider = endpointProvider;
        }

        public async Task<string> ExecuteAsync(string input, string systemProfile = "")
        {
            try
            {
                var ngrokEndpoint = _endpointProvider.GetNgrokEndpoint();

                var requestData = new
                {
                    model = "llama3.1",
                    stream = false,
                    format = "json",
                    prompt = input
                };

                var jsonRequest = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                _client.Timeout = Timeout.InfiniteTimeSpan;

                var response = await _client.PostAsync(ngrokEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    JsonDocument document = JsonDocument.Parse(jsonResponse);
                    JsonElement root = document.RootElement;
                    string responseJsonString = root.GetProperty("response").GetString() ?? "";

                    Console.WriteLine("----------OLLAMA RESPONSE: " + responseJsonString);
                    return responseJsonString;
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