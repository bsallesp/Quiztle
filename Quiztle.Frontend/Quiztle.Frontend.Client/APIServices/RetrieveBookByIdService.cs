using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Course;
using System.Net.Http.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class RetrieveBookByIdService
    {
        private readonly HttpClient _httpClient;

        public RetrieveBookByIdService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<Book>> ExecuteAsync(Guid bookId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/RetrieveBookById/{bookId}");

                if (response.IsSuccessStatusCode)
                {
                    var bookAPIResponse = await response.Content.ReadFromJsonAsync<APIResponse<Book>>();
                    return bookAPIResponse ?? new APIResponse<Book> { Status = CustomStatusCodes.ErrorStatus, Data = new() };
                }
                else
                {
                    return new APIResponse<Book> { Status = CustomStatusCodes.ErrorStatus, Data = new() };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<Book> { Status = CustomStatusCodes.ErrorStatus, Data = new() };
            }
        }
    }
}
