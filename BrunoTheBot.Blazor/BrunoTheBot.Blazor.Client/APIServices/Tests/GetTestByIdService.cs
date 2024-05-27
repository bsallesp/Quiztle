﻿using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using System.Text.Json;

namespace BrunoTheBot.Blazor.Client.APIServices.Tests
{
    public class GetTestByIdService
    {
        private readonly HttpClient _httpClient;

        public GetTestByIdService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<Test>> ExecuteAsync(Guid id)
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/GetTestById/" + id);
                Console.WriteLine(stringResponse);
                Test testsAPIResponse = JsonSerializer.Deserialize<Test>(stringResponse)!;

                return new APIResponse<Test>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = testsAPIResponse,
                    Message = "Total questions: " + testsAPIResponse.Questions.Count.ToString()
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