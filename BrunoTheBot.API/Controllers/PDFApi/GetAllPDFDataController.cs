using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.PDFData;
using BrunoTheBot.DataContext.Repositories.Quiz;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.CodeEntities;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class PDFDataController : ControllerBase
    {
        private readonly PDFDataRepository _pdfDataRepository;

        public PDFDataController(PDFDataRepository pdfDataRepository)
        {
            _pdfDataRepository = pdfDataRepository ?? throw new ArgumentNullException(nameof(pdfDataRepository));
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse<List<PDFData>>>> GetAllPDFData()
        {
            try
            {
                var pdfData = await _pdfDataRepository.GetAllPDFDataAsync();
                if (pdfData == null || pdfData.Count == 0)
                {
                    return Ok(new APIResponse<List<PDFData>>
                    {
                        Status = CustomStatusCodes.EmptyObjectErrorStatus,
                        Data = new List<PDFData>(),
                        Message = "No PDF data found."
                    });
                }

                return Ok(new APIResponse<List<PDFData>>
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    Data = pdfData,
                    Message = "Data retrieved successfully."
                });
            }
            catch (Exception ex)
            {
                // Log the exception details here using your logging framework
                return StatusCode(500, new APIResponse<List<PDFData>>
                {
                    Status = CustomStatusCodes.ErrorStatus,
                    Data = new List<PDFData>(),
                    Message = $"An error occurred while retrieving data: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PDFData>> GetPDFDataById(Guid id)
        {
            var pdfData = await _pdfDataRepository.GetPDFDataByIdAsync(id);
            if (pdfData == null)
            {
                return NotFound();
            }
            return Ok(pdfData);
        }

        [HttpPost]
        public async Task<ActionResult<PDFData>> CreatePDFData(PDFData pdfData)
        {
            try
            {
                await _pdfDataRepository.CreatePDFDataAsync(pdfData);
                return CreatedAtAction(nameof(GetPDFDataById), new { id = pdfData.Id }, pdfData);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePDFData(Guid id, PDFData pdfData)
        {
            if (id != pdfData.Id)
            {
                return BadRequest();
            }

            try
            {
                await _pdfDataRepository.UpdatePDFDataAsync(pdfData);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePDFData(Guid id)
        {
            var pdfData = await _pdfDataRepository.GetPDFDataByIdAsync(id);
            if (pdfData == null)
            {
                return NotFound();
            }

            await _pdfDataRepository.DeletePDFDataAsync(id);
            return NoContent();
        }
    }
}
