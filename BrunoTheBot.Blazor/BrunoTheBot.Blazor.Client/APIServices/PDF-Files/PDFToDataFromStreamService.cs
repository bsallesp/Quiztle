using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Utils;

namespace BrunoTheBot.Blazor.Client.APIServices
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
                Console.WriteLine($"{DateTime.Now} Complete URI: {fullUrl}");

                var response = await _httpClient.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{DateTime.Now} Response status code: {response.StatusCode}");

                return new APIResponse<string>
                {
                    Data = responseData,
                    Message = "PDFDataFromStreamService: Processing complete",
                    Status = CustomStatusCodes.SuccessStatus
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} Exception occurred: {ex.Message}");
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
