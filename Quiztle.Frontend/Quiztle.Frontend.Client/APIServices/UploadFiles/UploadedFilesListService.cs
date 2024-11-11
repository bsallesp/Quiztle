using Quiztle.CoreBusiness.APIEntities;
using Quiztle.CoreBusiness.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Quiztle.Blazor.Client.APIServices
{
    public class UploadedFilesListService
    {
        private readonly HttpClient _httpClient;

        public UploadedFilesListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<APIResponse<PDFFileListResponse>> ExecuteAsync()
        {
            try
            {
                var url = "api/PDFFileList/list-files";
                var stringResponse = await _httpClient.GetStringAsync(url);
                APIResponse<PDFFileListResponse> filesAPIResponse =
                    JsonSerializer.Deserialize<APIResponse<PDFFileListResponse>>(stringResponse)
                    ?? new APIResponse<PDFFileListResponse> { Data = new PDFFileListResponse() };
                return filesAPIResponse;
            }
            catch (HttpRequestException httpEx)
            {
                return new APIResponse<PDFFileListResponse>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new PDFFileListResponse(),
                    Message = "A network error occurred while retrieving files."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<PDFFileListResponse>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new PDFFileListResponse(),
                    Message = "An error occurred while retrieving files."
                };
            }
        }
    }

    public class PDFFileListResponse
    {
        [JsonPropertyName(nameof(FileNames))]
        public List<string> FileNames { get; set; } = [];

        [JsonPropertyName(nameof(FilePaths))]
        public List<string> FilePaths { get; set; } = [];
    }
}