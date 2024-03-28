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
            try
            {
                var data = new
                {
                    inputs = inputText,
                    parameters = new
                    {
                        temperature = 0.2,
                        max_length = 50000,
                        top_k = 40,
                        top_p = 0.9,
                        num_return_sequences = 1
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

                string[] model = new string[]
                {
                    "meta-llama/Llama-2-7b-chat-hf",
                    "deepseek-ai/deepseek-vl-7b-base",
                    "NousResearch/Nous-Hermes-2-Mixtral-8x7B-DPO",
                    "mistralai/Mixtral-8x7B-Instruct-v0.1"
                };

                var url = "https://api-inference.huggingface.co/models/";
                var finalurl = url + model[3];

                var response = await _httpClient.PostAsync(finalurl, content);

                Console.WriteLine("Using " + finalurl);

                var responseContent = await response.Content.ReadAsStringAsync();

                // Verificar se a requisição foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    // Processar a resposta e extrair o texto gerado
                    // ...
                    return responseContent; // Retornar o texto gerado
                }
                else
                {
                    // Se a requisição não for bem-sucedida, lançar uma exceção com detalhes do erro
                    throw new Exception($"Erro ao gerar texto: {(int)response.StatusCode} - {response.ReasonPhrase}. Detalhes: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                // Se ocorrer um erro durante a execução do método, lançar uma exceção com detalhes do erro
                throw new Exception("Erro ao gerar texto", ex);
            }
        }

    }
}

// HF TOKEN
//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

//DEEPSEEK
//sk-62c786ca9d7f49ffa383ac9815799642
