using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.DataContext.Repositories.Quiz;

namespace BrunoTheBot.API.Controllers.PDFApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetQuestionsController : ControllerBase
    {
        private readonly PDFDataRepository _pDFDataRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly ChatGPTRequest _chatGPTRequest;

        public GetQuestionsController(PDFDataRepository pDFDataRepository, QuestionRepository questionRepository, ChatGPTRequest chatGPTRequest)
        {
            _pDFDataRepository = pDFDataRepository;
            _questionRepository = questionRepository;
            _chatGPTRequest = chatGPTRequest;
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteAsync(Guid id, int startPage, int endPage)
        {
            try
            {
                var pdfData = await _pDFDataRepository.GetPDFDataByIdAsyncByPage(id, startPage, endPage);
                if (pdfData == null) return NotFound("PDF data is null");



                return Ok(pdfData);
            }
            catch(Exception ex)
            {
                return BadRequest($"Error deserializing JSON: {ex.Message}");
            }
        }
    }
}
