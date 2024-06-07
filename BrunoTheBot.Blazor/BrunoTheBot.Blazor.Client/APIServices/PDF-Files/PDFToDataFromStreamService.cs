using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Utils;
using System.Net.Http.Json;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class PDFToDataFromStreamService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url = "api/CreatePDFDataFromStream";
        private readonly string _fileName = "model.pdf";

        public PDFToDataFromStreamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<bool>> ExecuteAsync(string fileName, string pdfDataName)
        {
            try
            {
                var fileData = new
                {
                    fileName = fileName,
                    name = pdfDataName
                };

                var response = await _httpClient.PostAsJsonAsync(_url, fileData);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return new APIResponse<bool>
                    {
                        Data = true,
                        Message = "PDFToDataFromStreamService: " + "Processing complete",
                        Status = CustomStatusCodes.SuccessStatus
                    };
                }
                else
                {
                    return new APIResponse<bool>
                    {
                        Data = false,
                        Message = "PDFToDataFromStreamService: " + "Not found",
                        Status = CustomStatusCodes.NotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse<bool>
                {
                    Data = false,
                    Message = "PDFToDataFromStreamService: " + $"An error occurred: {ex.Message} \n + {fileName}",
                    Status = CustomStatusCodes.ErrorStatus
                };
            }
        }
    }
}
