using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using System.Text.Json;

namespace BrunoTheBot.Blazor.Client.APIServices.Tests
{
    public class GetAllTestsByPDFDataIdService
    {
        private readonly HttpClient _httpClient;

        public GetAllTestsByPDFDataIdService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<List<Test>>> ExecuteAsync(Guid id)
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetAllTestsByPDFDataId/" + id);
                Console.WriteLine(stringResponse);
                APIResponse<List<Test>> booksAPIResponse = JsonSerializer.Deserialize<APIResponse<List<Test>>>(stringResponse)!;
                return booksAPIResponse;
            }
            catch
            {
                return new APIResponse<List<Test>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Test>()
                };
            }
        }
    }
}