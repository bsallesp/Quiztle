using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using System.Net.Http.Json;
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

        public async Task<BooksAPIResponse> ExecuteAsync(string bookName, int chaptersAmount = 5, int sectionsAmount = 5)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/CreateBook/CreateBookController",
                    new { bookName, chaptersAmount, sectionsAmount });
                response.EnsureSuccessStatusCode();

                var stringResponse = await response.Content.ReadAsStringAsync();
                var booksAPIResponse = JsonSerializer.Deserialize<BooksAPIResponse>(stringResponse);

                return booksAPIResponse ?? new BooksAPIResponse
                {
                    Status = CustomStatusCodes.EmptyObjectErrorStatus,
                    Books = new List<CoreBusiness.Entities.Course.Book>()
                };
            }
            catch
            {
                return new BooksAPIResponse
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Books = null
                };
            }
        }
    }
}
