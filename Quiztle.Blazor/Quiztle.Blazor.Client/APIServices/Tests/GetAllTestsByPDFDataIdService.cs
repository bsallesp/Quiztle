using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices.Tests
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
                APIResponse<List<Test>> testsAPIResponse = JsonSerializer.Deserialize<APIResponse<List<Test>>>(stringResponse)!;
                return testsAPIResponse;
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