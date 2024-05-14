using System.Net;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class CreateBookTaskService
    {
        private readonly HttpClient _httpClient;

        public CreateBookTaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ExecuteAsync(string bookName)
        {
            try
            {
                string encodedBookName = WebUtility.UrlEncode(bookName);
                var endpoint = $"api/CreateBookTask/CreateBookTaskController/{encodedBookName}/";
                Console.WriteLine(_httpClient.BaseAddress + endpoint);
                var result = await _httpClient.GetAsync(endpoint);

                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Tarefa criada com sucesso.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Erro ao criar o livro: {result.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar solicitação HTTP: {ex.Message}");
                return false;
            }
        }
    }
}