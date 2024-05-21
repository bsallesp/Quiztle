using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFToJsonController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PDFToJsonController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync([FromBody] string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return BadRequest("File path not provided");

            var client = _httpClientFactory.CreateClient();

            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(filePath), "file_path");
                content.Add(new StringContent("1"), "partial_output_rate"); // example rate

                var response = await client.PostAsync("http://localhost:5090/extract-text", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
