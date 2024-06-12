using System.Net;

namespace Quiztle.Blazor.Client.APIServices
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
                var result = await _httpClient.GetAsync(endpoint);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"CreateBookTaskService: Erro ao criar o livro: {result.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"CreateBookTaskService: Erro ao executar solicitação HTTP: {ex.Message}");
            }
        }
    }
}