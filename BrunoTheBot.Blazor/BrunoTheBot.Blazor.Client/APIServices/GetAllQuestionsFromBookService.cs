using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using System.Text.Json;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class GetAllQuestionsFromBookService
    {
        private HttpClient _httpClient;

        public GetAllQuestionsFromBookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookAPIResponse> ExecuteAsync(int bookId)
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("/api/RetrieveBookById/" + bookId);

                BookAPIResponse booksAPIResponse = JsonSerializer.Deserialize<BookAPIResponse>(stringResponse)!;

                return booksAPIResponse;
            }
            catch
            {
                return new BookAPIResponse
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Book = new Book()
                };
            }
        }
    }
}