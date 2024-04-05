using BrunoTheBot.API.Controllers.FromLLMControllers;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.HeadControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFullSchoolCourse : ControllerBase
    {
        private readonly SchoolRepository _schoolDb;

        public GetFullSchoolCourse(SchoolRepository schoolDb, FromLLMToLogController requestLLMController)
        {
            _schoolDb = schoolDb;
        }

        [HttpPost("GetSchoolTopicsAuthors")]
        public async Task<ActionResult<string>> GetSchoolFeatures([FromBody] string input, int subTopicsAmount)
        {
            try
            {
                School _school = new School();

                var subTopicsResponse = await _requestLLMController.GetSubTopics(input, subTopicsAmount);

                await _schoolDb.CreateSchoolAsync(new School
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
