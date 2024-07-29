using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices.Tests
{
    public class GetAllTestsService
    {
        private readonly HttpClient _httpClient;

        public GetAllTestsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<List<Test>>> ExecuteAsync()
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetAllTests");
                List<Test> dataTemp = JsonSerializer.Deserialize<List<Test>>(stringResponse)!;

                foreach(var item in dataTemp)
                {
                    Console.WriteLine(item.Name);
                }

                APIResponse<List<Test>> apiResult = new() { Data = dataTemp, Message = "Total: " + dataTemp.Count};

                return apiResult;
            }
            catch (Exception ex)
            {
                return new APIResponse<List<Test>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Test>(),
                    Message = "error in GetAllTestsService " + DateTime.UtcNow + "\n" + ex.Message.ToString()
                };
            }
        }
    }
}