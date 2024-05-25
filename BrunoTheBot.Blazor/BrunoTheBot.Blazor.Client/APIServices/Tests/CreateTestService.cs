using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using System;
using System.Text.Json;

namespace BrunoTheBot.Blazor.Client.APIServices.Tests
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

//https://localhost:7204/api/CreateTestFromPDFDataPages?id=ca940e74-e772-44d5-b12e-1b5c34b38fa2&name=Short%20Test&startPage=300&endPage=307


//https://localhost:7204/api/CreateTestFromPDFDataPages/id=ca940e74-e772-44d5-b12e-1b5c34b38fa2&name=From%20250%20to%20260&startPage=250&endPage=260