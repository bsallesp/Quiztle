using BrunoTheBot.CoreBusiness.Entities;
using BrunoTheBot.DataContext.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.HeadControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetSchoolTopicsAuthorsController : ControllerBase
    {
        private readonly SchoolRepository _schoolDb;
        private readonly FromLLMToLogController _requestLLMController;

        public GetSchoolTopicsAuthorsController(SchoolRepository schoolDb, FromLLMToLogController requestLLMController)
        {
            _schoolDb = schoolDb;
            _requestLLMController = requestLLMController;
        }

        [HttpPost("GetSchoolTopicsAuthors")]
        public async Task<ActionResult<string>> GetSchoolFeatures([FromBody] string input, int subTopicsAmount)
        {
            try
            {
                School _school = new School();

                var subTopicsResponse = await _requestLLMController.GetSubTopics(input, subTopicsAmount);
                
                

                await _schoolDb.CreateSchoolAsync(new CoreBusiness.Entities.School
                {

                });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao interagir com a API ChatGPT: {ex.Message}");
            }
        }
    }
}
