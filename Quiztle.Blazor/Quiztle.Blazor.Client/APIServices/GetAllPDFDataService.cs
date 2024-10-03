using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Quiztle.CoreBusiness.Entities.PDFData;
using Newtonsoft.Json;

namespace Quiztle.Blazor.Client.APIServices
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
                APIResponse<List<PDFData>> apiResponse = JsonConvert.DeserializeObject<APIResponse<List<PDFData>>>(stringResponse)!;

                if (apiResponse == null || apiResponse.Data == null || apiResponse.Data.Count == 0)
                {
                    return new APIResponse<List<PDFData>>
                    {
                        Status = CustomStatusCodes.NotFound,
                        Data = new List<PDFData>(),
                        Message = "No data or response object is null."
                    };
                }

                return apiResponse;
            }
            catch (Exception ex)
            {
                string innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                string detailedErrorMessage = $"Exception Message: {ex.Message}; " +
                                              $"Inner Exception: {innerExceptionMessage}; " +
                                              $"Stack Trace: {ex.StackTrace}";

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
