//https://localhost:7204/api/RemoveTest/17adeada-fa32-4654-8092-255d308ee95d


using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Quiz;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Quiztle.Blazor.Client.APIServices.Tests
{
    public class RemoveTestService
    {
        private readonly HttpClient _httpClient;

        public RemoveTestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<bool>> ExecuteAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync("api/RemoveTest/" + id);
                
                var isRemoved = bool.Parse(response);

                if (!isRemoved) return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = "error: "
                };

                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = false,
                    Message = "ok"
                };

            }
            catch
            {
                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = "error: "
                };
            }
        }
    }
}