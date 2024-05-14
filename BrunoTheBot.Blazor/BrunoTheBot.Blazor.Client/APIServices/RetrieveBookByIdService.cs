using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using System.Net.Http.Json;

namespace BrunoTheBot.Blazor.Client.APIServices
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
                    // Se a solicitação não for bem-sucedida, retorne uma resposta de erro
                    return new APIResponse<Book> { Status = CustomStatusCodes.ErrorStatus, Data = new() };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar a solicitação HTTP: {ex.Message}");
                return new APIResponse<Book> { Status = CustomStatusCodes.ErrorStatus, Data = new() };
            }
        }
    }
}
