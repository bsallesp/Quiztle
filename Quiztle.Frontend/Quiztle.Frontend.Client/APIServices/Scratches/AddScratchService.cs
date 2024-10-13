using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.Scratch;
using System.Net.Http.Json;

namespace Quiztle.Blazor.Client.APIServices
{
    public class AddScratchService
    {
        private readonly HttpClient _httpClient;

        public AddScratchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<Scratch>> ExecuteAsync(Scratch scratch)
        {
            try
            {
                var url = "api/AddScratch/";

                var stringResponse = await _httpClient.PostAsJsonAsync(url, scratch);

                if (!stringResponse.IsSuccessStatusCode)
                {
                    return new APIResponse<Scratch>
                    {
                        Status = CustomStatusCodes.ErrorStatus,
                        Data = new(),
                        Message = "ERROR IN PostAsJsonAsync - AddScratchService."
                    };
                }

                return new APIResponse<Scratch>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = scratch,
                    Message = "Sucess sending scratches: "
                };

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.Data.ToString());

                return new APIResponse<Scratch>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new(),
                    Message = ex.Message + "ERROR IN PostAsJsonAsync - AddScratchService " + ex.Data.ToString()
                };
            }
        }
    }
}