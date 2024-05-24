using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using BrunoTheBot.CoreBusiness.Entities.PDFData;
using Newtonsoft.Json;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class GetAllPDFDataService
    {
        private readonly HttpClient _httpClient;

        public GetAllPDFDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<APIResponse<List<PDFData>>> ExecuteAsync()
        {
            try
            {
                var stringResponse = await _httpClient.GetStringAsync("api/PDFData");
                Console.WriteLine(stringResponse);
                APIResponse<List<PDFData>> apiResponse = JsonConvert.DeserializeObject<APIResponse<List<PDFData>>>(stringResponse)!;

                if (apiResponse == null || apiResponse.Data == null || apiResponse.Data.Count == 0)
                {
                    return new APIResponse<List<PDFData>>
                    {
                        Status = CustomStatusCodes.EmptyObjectErrorStatus,
                        Data = new List<PDFData>(),
                        Message = "No data or response object is null."
                    };
                }

                Console.WriteLine("SUCESS!! " + apiResponse.Data.Count);

                return apiResponse;
            }
            catch (Exception ex)
            {
                string innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                string detailedErrorMessage = $"Exception Message: {ex.Message}; " +
                                              $"Inner Exception: {innerExceptionMessage}; " +
                                              $"Stack Trace: {ex.StackTrace}";
                Console.WriteLine(detailedErrorMessage);

                return new APIResponse<List<PDFData>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<PDFData>(),
                    Message = detailedErrorMessage
                };
            }
        }
    }
}
