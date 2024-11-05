using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class GetTestByIdService(HttpClient httpClient)
    {
        public async Task<APIResponse<Test>> ExecuteAsync(Guid id)
        {
            try
            {
                var stringResponse = await httpClient.GetStringAsync("api/GetTestById/" + id);
                Test testResponse = JsonSerializer.Deserialize<Test>(stringResponse)!;

                return new APIResponse<Test>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = testResponse,
                    Message = "Total questions: " + testResponse.Questions.Count.ToString()
                };
                
            }
            catch
            {
                return new APIResponse<Test>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Test()
                };
            }
        }
    }
}