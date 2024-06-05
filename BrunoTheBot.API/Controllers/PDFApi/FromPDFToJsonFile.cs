using Microsoft.AspNetCore.Mvc;
namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FromPDFToJsonFile : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _pdfApiUrl;

        public FromPDFToJsonFile(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _pdfApiUrl = Environment.GetEnvironmentVariable("PDF_API_URL") ?? configuration["PDF_API_URL"] ?? throw new Exception("ENV VARIABLE NOT FOUND");
            if (string.IsNullOrEmpty(_pdfApiUrl))
            {
                throw new Exception("PDF_API_URL is not set");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return BadRequest("File path not provided");

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(30); // Adjust timeout according to your needs

            try
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(filePath), "file_path" },
                    { new StringContent("1"), "partial_output_rate" } // Assuming you want to process every page
                };

                var request = new HttpRequestMessage(HttpMethod.Post, $"{_pdfApiUrl}/extract-text") { Content = content };
                using var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Received response: " + responseContent);

                return Ok(responseContent);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
