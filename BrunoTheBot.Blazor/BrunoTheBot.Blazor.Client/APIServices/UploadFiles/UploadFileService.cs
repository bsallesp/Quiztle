using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Forms;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class UploadFileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _uploadEndpoint = "api/uploadfile/upload"; // ou o caminho relativo apropriado

        public UploadFileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            content.Add(content: fileContent, name: "file", fileName: file.Name);

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
