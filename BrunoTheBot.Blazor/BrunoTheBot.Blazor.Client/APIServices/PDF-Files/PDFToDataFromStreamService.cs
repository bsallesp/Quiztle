using System;
using System.Net.Http;
using System.Net.Http.Json;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Utils;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class PDFToDataFromStreamService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseServiceUrl = "api/CreatePDFDataFromStream"; 

        public PDFToDataFromStreamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<string>> CreatePDFDataAsync(string fileName, string pdfDataName)
        {
            try
            {
                // Construa a query string com os parâmetros adequados
                var queryString = $"?fileName={Uri.EscapeDataString(fileName)}&name={Uri.EscapeDataString(pdfDataName)}";
                var fullUrl = $"{_httpClient.BaseAddress}{_baseServiceUrl}{queryString}";
                Console.WriteLine($"{DateTime.Now} Complete URI: {fullUrl}");
                Console.WriteLine($"{DateTime.Now} Sending request to {fullUrl}");

                // Como os parâmetros agora estão na URL, você enviará um request vazio para o PostAsync
                var response = await _httpClient.PostAsync(fullUrl, null);
                Console.WriteLine($"{DateTime.Now} Request sent. Waiting for response...");

                response.EnsureSuccessStatusCode(); // Isso lançará uma exceção se o status code da resposta não for de sucesso

                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{DateTime.Now} Response status code: {response.StatusCode}");
                Console.WriteLine($"{DateTime.Now} Server response: {responseData}");

                if (response.IsSuccessStatusCode)
                {
                    return new APIResponse<string>
                    {
                        Data = responseData,
                        Message = "PDFDataFromStreamService: Processing complete",
                        Status = CustomStatusCodes.SuccessStatus
                    };
                }
                else
                {
                    return new APIResponse<string>
                    {
                        Data = "",
                        Message = "PDFDataFromStreamService: Request failed",
                        Status = CustomStatusCodes.NotFound
                    };
                }
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
