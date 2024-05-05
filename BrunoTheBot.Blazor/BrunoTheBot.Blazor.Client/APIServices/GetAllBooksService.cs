using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using System.Text.Json;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class GetAllBooksService
    {
        private readonly HttpClient _httpClient;

        public GetAllBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BooksAPIResponse> ExecuteAsync()
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetAllBooks/GetAllBooksController");

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
