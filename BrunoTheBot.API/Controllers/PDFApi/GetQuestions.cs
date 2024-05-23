using System.Text.Json;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BrunoTheBot.API.Prompts;
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
            var pdfData = await _pDFDataRepository.GetPDFDataByIdAsyncByPage(id, startPage, endPage);
            if (pdfData == null) throw new Exception("PDF data is null");

            return Ok(pdfData);

            //if (selectedPages.Count <= 0) throw new Exception("No selectedPages found");

            //foreach (var item in selectedPages)
            //{
            //    Console.WriteLine(item);
            //}



            //var prompt = QuestionsPrompts.GetQuestionsFromPages(pages, startPage, endPage, pages.Count);
            //var jsonResult = await _chatGPTRequest.ExecuteAsync(prompt);

            //Console.WriteLine(JsonSerializer.Serialize(jsonResult, new JsonSerializerOptions { WriteIndented = true }));

            //try
            //{
            //    var questions = JsonSerializer.Deserialize<List<Question>>(jsonResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //    return Ok(questions);
            //}
            //catch (JsonException ex)
            //{
            //    return BadRequest($"Error deserializing JSON: {ex.Message}");
            //}
        }
    }
}
