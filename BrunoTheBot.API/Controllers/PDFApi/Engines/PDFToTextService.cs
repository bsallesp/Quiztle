using System.Net.Http.Headers;

namespace BrunoTheBot.API.Controllers.PDFApi.Engines
{
    public class PDFToTextService
    {
        private readonly HttpClient _httpClient;

        public PDFToTextService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ExecuteAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File not found");

            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();
            using var streamContent = new StreamContent(fileStream);
            using var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync("api/extractText", content);

            if (response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                Console.WriteLine(text);
                return text;
            }
            else
            {
                throw new Exception($"Erro ao converter PDF para texto: {response.StatusCode}");
            }
        }
    }
}