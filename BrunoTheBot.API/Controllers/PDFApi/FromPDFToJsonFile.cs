using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FromPDFToJsonFile : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FromPDFToJsonFile(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5090/extract-text") { Content = content };
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
