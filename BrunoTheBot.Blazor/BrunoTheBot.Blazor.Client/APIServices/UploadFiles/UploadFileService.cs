using System.Net.Http.Headers;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class UploadFileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _uploadEndpoint;

        public UploadFileService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var environment = configuration["ASPNETCORE_ENVIRONMENT"];
            _uploadEndpoint = environment == "Development"
                ? "http://localhost:5514/api/uploadfile/upload"
                : "https://localhost:5514/api/uploadfile/upload";

            Console.WriteLine($"UploadFileService initialized with endpoint: {_uploadEndpoint}");
        }

        public async Task<string> UploadAsync(IBrowserFile file)
        {
            Console.WriteLine("UploadAsync started.");

            if (file == null || file.Size == 0)
            {
                Console.WriteLine("No file selected or file is empty.");
                throw new InvalidOperationException("No file selected or file is empty.");
            }

            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(content: fileContent, name: "\"file\"", fileName: file.Name);

            Console.WriteLine($"Uploading file: {file.Name} to {_uploadEndpoint}");

            var response = await _httpClient.PostAsync(_uploadEndpoint, content);
            Console.WriteLine($"Response status code: {response.StatusCode}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("File uploaded successfully.");
            Console.WriteLine($"Server response: {responseContent}");

            return responseContent;
        }
    }
}
