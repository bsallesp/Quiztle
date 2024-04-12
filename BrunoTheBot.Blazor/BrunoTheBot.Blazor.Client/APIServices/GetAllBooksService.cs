using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using System.Net.Http.Json;
using System.Text.Json;

namespace BrunoTheBot.Blazor.APIServices
{
    public class GetAllBooksService
    {
        private readonly HttpClient _httpClient;

        public GetAllBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("http://localhost:5044");
        }

        public async Task<BooksAPIResponse> ExecuteAsync()
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetAllBooks/GetAllBooks");

                BooksAPIResponse booksAPIResponse = JsonSerializer.Deserialize<BooksAPIResponse>(stringResponse)!;

                return booksAPIResponse;
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
