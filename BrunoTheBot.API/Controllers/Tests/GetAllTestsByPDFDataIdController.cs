using BrunoTheBot.CoreBusiness.Entities.PDFData;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using BrunoTheBot.DataContext.DataService.Repository.Quiz;
using BrunoTheBot.DataContext.Repositories.Quiz;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.Tests
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetAllTestsByPDFDataIdController : ControllerBase
    {
        private readonly TestRepository _testRepository;

        public GetAllTestsByPDFDataIdController(TestRepository testRepository)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Test>>> ExecuteAsync(Guid id)
        {
            var response = await _testRepository.GetAllTestsByPDFDataIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}