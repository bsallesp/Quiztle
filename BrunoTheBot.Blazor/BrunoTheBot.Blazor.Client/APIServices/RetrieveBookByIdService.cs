using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
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

        public async Task<BookAPIResponse> ExecuteAsync(int bookId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/RetrieveBookById/{bookId}");

                if (response.IsSuccessStatusCode)
                {
                    var booksAPIResponse = await response.Content.ReadFromJsonAsync<BookAPIResponse>();
                    return booksAPIResponse ?? new BookAPIResponse { Status = CustomStatusCodes.ErrorStatus, Book = new() };
                }
                else
                {
                    // Se a solicitação não for bem-sucedida, retorne uma resposta de erro
                    return new BookAPIResponse { Status = CustomStatusCodes.ErrorStatus, Book = new() };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar a solicitação HTTP: {ex.Message}");
                return new BookAPIResponse { Status = CustomStatusCodes.ErrorStatus, Book = new() };
            }
        }
    }
}
