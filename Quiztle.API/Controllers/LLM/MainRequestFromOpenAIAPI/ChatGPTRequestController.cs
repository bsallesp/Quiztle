using Microsoft.AspNetCore.Mvc;
using Quiztle.API.Controllers.LLM.Interfaces;
using Quiztle.CoreBusiness.Log;
using Quiztle.DataContext.Repositories;
using System.Text;
using System.Text.Json;

namespace Quiztle.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatGPTRequestController : ControllerBase, ILLMChatGPTRequest, IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly AILogRepository _logRepository;

        public ChatGPTRequestController(HttpClient client, AILogRepository aILogRepository)
        {
            _client = client;
            _apiKey = "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";
            _logRepository = aILogRepository;
        }

        [HttpPost("chatgpt")]
        public async Task<string> ExecuteAsync(string input, string systemProfile = "")
        {
            try
            {
                if (string.IsNullOrEmpty(systemProfile))
                    systemProfile = "You are a helpful assistant specialized in crafting educational content," +
                                    " possessing comprehensive understanding across all study-related domains." +
                                    " You consistently provide structured JSON responses," +
                                    " ensuring clarity and consistency in information delivery";

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

                if (!_client.DefaultRequestHeaders.Any())
                    _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                await _logRepository.CreateAILogAsync(new AILog
                {
                    JSON = jsonRequest,
                    Name = "ChatGPTRequestController"
                });

                var response = await _client.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    using JsonDocument document = JsonDocument.Parse(jsonResponse);

                    // Acessa o campo que contém as perguntas
                    var questions = document.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .ToString();

                    // Retorna apenas as perguntas
                    Console.WriteLine(questions);

                    return questions;
                }
                else
                {
                    string errorMessage = $"Erro ao fazer a solicitação para a API ChatGPT. Código de status: {response.StatusCode}. ";
                    errorMessage += $"Motivo: {response.ReasonPhrase}. Conteúdo da resposta: {await response.Content.ReadAsStringAsync()}";
                    Console.WriteLine(errorMessage);
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


        [HttpPost("chatgpt/simple")]
        public async Task<string> ExecuteAsync(string input)
        {
            var jsonResponse = await ExecuteAsync(input, string.Empty);
            using JsonDocument document = JsonDocument.Parse(jsonResponse);
            var messageContent = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return messageContent?.Trim()!;
        }

        public void Dispose() => _client.Dispose();
    }
}