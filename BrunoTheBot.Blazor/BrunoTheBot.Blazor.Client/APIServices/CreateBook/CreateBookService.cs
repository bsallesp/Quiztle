using System.Text.Json;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class CreateBookService
    {
        private readonly HttpClient _httpClient;

        public CreateBookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ExecuteAsync(string bookName, int charptersAmount = 5, int sectionsAmount = 5)
        {
            try
            {
                var requestData = new
                {
                    bookName = bookName,
                    chaptersAmount = charptersAmount,
                    sectionsAmount = sectionsAmount
                };

                var jsonData = JsonSerializer.Serialize(requestData);
                // Modifique a URL para corresponder à que você forneceu
                var response = await _httpClient.GetAsync($"api/CreateBook/CreateBookTaskController/{bookName}/");



                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Lidar com exceções
                Console.WriteLine($"Erro ao executar solicitação HTTP: {ex.Message}");
                return string.Empty;
            }
        }
    }
}



//using BrunoTheBot.CoreBusiness.APIEntities;
//using BrunoTheBot.CoreBusiness.CodeEntities;
//using System.Net.Http.Json;
//using System.Text.Json;

//namespace BrunoTheBot.Blazor.Client.APIServices
//{
//    public class CreateBookService
//    {
//        private readonly HttpClient _httpClient;

//        public CreateBookService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<BooksAPIResponse> ExecuteAsync(string bookName, int chaptersAmount = 5, int sectionsAmount = 5)
//        {
//            try
//            {
//                var response = await _httpClient.PostAsJsonAsync("api/CreateBook/CreateBook/",
//                    new { bookName, chaptersAmount, sectionsAmount });
//                response.EnsureSuccessStatusCode();

//                var stringResponse = await response.Content.ReadAsStringAsync();
//                var booksAPIResponse = JsonSerializer.Deserialize<BooksAPIResponse>(stringResponse);

//                return booksAPIResponse ?? new BooksAPIResponse
//                {
//                    Status = CustomStatusCodes.EmptyObjectErrorStatus,
//                    Books = new List<CoreBusiness.Entities.Course.Book>()
//                };
//            }
//            catch (Exception ex)
//            {
//                // Captura e retorna informações detalhadas da exceção
//                string errorMessage = $"Ocorreu uma exceção no CreatBookService: {ex.Message}";

//                // Verifica se a exceção possui uma causa (InnerException)
//                if (ex.InnerException != null)
//                {
//                    errorMessage += $" InnerException: {ex.InnerException.Message}";
//                }

//                // Adiciona outras propriedades da exceção, se necessário
//                errorMessage += $" StackTrace: {ex.StackTrace}";

//                // Lança uma nova exceção com a mensagem detalhada
//                throw new Exception(errorMessage);
//            }
//        }
//    }
//}
