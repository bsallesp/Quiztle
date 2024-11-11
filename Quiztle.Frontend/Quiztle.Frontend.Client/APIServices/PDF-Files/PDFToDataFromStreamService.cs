using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;

namespace Quiztle.Blazor.Client.APIServices
{
    public class PDFToDataFromStreamService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseServiceUrl = "api/CreatePDFDataFromStream";

        public PDFToDataFromStreamService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<APIResponse<string>> CreatePDFDataAsync(string fileName, string pdfDataName, int partialOutputRate = 1)
        {
            try
            {
                var queryString = $"?fileName={Uri.EscapeDataString(fileName)}&pdfDataName={Uri.EscapeDataString(pdfDataName)}&partialOutputRate={partialOutputRate}";
                var fullUrl = $"{_httpClient.BaseAddress}{_baseServiceUrl}{queryString}";
                var response = await _httpClient.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                return new APIResponse<string>
                {
                    Data = responseData,
                    Message = "PDFDataFromStreamService: Processing complete",
                    Status = CustomStatusCodes.SuccessStatus
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Data = "",
                    Message = $"PDFDataFromStreamService: An error occurred - {ex.Message}",
                    Status = CustomStatusCodes.ErrorStatus
                };
            }
        }
    }
}
