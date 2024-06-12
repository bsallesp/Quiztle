using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System;
using System.Text.Json;

namespace Quiztle.Blazor.Client.APIServices.Tests
{
    public class CreateTestService
    {
        private readonly HttpClient _httpClient;

        public CreateTestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<Test>> ExecuteAsync(Guid id, string name  = "unnamed", int startPage = 1, int endPage = 20)
        {
            try
            {
                var url = "api/CreateTestFromPDFDataPages/"
                    + "?id=" + id
                    + "&name=" + Uri.EscapeDataString(name)
                    + "&startPage=" + startPage.ToString()
                    + "&endPage=" + endPage.ToString();

                Console.WriteLine(url);


                var stringResponse = await _httpClient.GetStringAsync(url);
                
                Test testsAPIResponse = JsonSerializer.Deserialize<Test>(stringResponse)!;

                return new APIResponse<Test>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = testsAPIResponse,
                    Message = "Total questions: " + testsAPIResponse.Questions.Count.ToString()
                };

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.Data.ToString());

                return new APIResponse<Test>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new Test(),
                    Message = ex.Message + " " + ex.Data.ToString()
                };
            }
        }
    }
}