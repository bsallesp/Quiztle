using System.Net.Http.Headers;
using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using Microsoft.AspNetCore.Components.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Quiztle.Blazor.Client.APIServices
{
    public class UploadFileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _uploadEndpoint = "api/uploadfile/upload";

        public UploadFileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<bool>> UploadAsync(IBrowserFile file)
        {
            try
            {
                if (file == null || file.Size == 0)
                {
                    throw new InvalidOperationException("No file selected or file is empty.");
                }

                using var content = new MultipartFormDataContent();
                using var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(content: fileContent, name: "file", fileName: file.Name);
                var response = await _httpClient.PostAsync(_uploadEndpoint, content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = true,
                    Message = "UploadFileService: " + response.EnsureSuccessStatusCode().ToString()
                };

            }
            catch(Exception ex)
            {
                return new APIResponse<bool>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = false,
                    Message = "UploadFileService: error: " + ex.Message
                };
            }
        }
    }
}
