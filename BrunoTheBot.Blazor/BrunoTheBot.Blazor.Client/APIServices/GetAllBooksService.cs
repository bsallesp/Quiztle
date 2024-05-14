using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
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

        public async Task<APIResponse<List<Book>>> ExecuteAsync()
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetAllBooks/GetAllBooksController");
                APIResponse<List<Book>> booksAPIResponse = JsonSerializer.Deserialize<APIResponse<List<Book>>>(stringResponse)!;

                return booksAPIResponse;
            }
            catch
            {
                return new APIResponse<List<Book>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Book>()
                };
            }
        }
    }
}
