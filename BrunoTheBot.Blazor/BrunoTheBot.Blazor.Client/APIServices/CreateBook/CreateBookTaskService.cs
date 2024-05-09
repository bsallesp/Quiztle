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
                var endpoint = $"api/CreateBookTask/CreateBookTaskController/{bookName}/";
                Console.WriteLine(_httpClient.BaseAddress + endpoint);
                var result = await _httpClient.GetAsync(endpoint);

                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Livro criado com sucesso.");
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


//https://localhost:7204/api/CreateBookTaskController/MIT%20Computer%20Science%20Full%20Course/
//https://localhost:7204/api/CreateBookTask/CreateBookTaskController/{bookName}