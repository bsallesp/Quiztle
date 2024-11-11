using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Scratch;
using System.Net.Http.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class GetAllScratchesService
    {
        private readonly HttpClient _httpClient;

        public GetAllScratchesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<List<Scratch>>> ExecuteAsync()
        {
            try
            {
                var url = "api/GetAllScratches/all";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new APIResponse<List<Scratch>>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = new List<Scratch>(),
                        Message = "ERROR IN GetAsync - GetAllScratchesService."
                    };
                }

                var scratches = await response.Content.ReadFromJsonAsync<List<Scratch>>();
                return new APIResponse<List<Scratch>>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = scratches ?? [],
                    Message = "Successfully retrieved all scratches."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<List<Scratch>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<Scratch>(),
                    Message = ex.Message + " ERROR IN GetAsync - GetAllScratchesService " + ex.Data?.ToString()
                };
            }
        }
    }
}
