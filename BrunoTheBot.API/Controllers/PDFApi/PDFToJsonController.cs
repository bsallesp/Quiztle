using BrunoTheBot.CoreBusiness.Entities.Tasks;
using BrunoTheBot.DataContext.DataService.Repository.Tasks;
using BrunoTheBot.API.Controllers.PDFApi.Engines;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFToJsonController : ControllerBase
    {
        private readonly PDFToTextService _pdfToTextService;

        public PDFToJsonController(PDFToTextService pdfToTextService)
        {
            _pdfToTextService = pdfToTextService;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not found");

            try
            {
                var text = await _pdfToTextService.ExecuteAsync(file);
                Console.WriteLine(text);
                return Ok(text);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
