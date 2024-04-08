using BrunoTheBot.API.Controllers.FromLLMToDBControllers;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.Course;


namespace BrunoTheBot.API.Controllers.HeadControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFullSchoolCourse(SchoolRepository schoolDb, TopicClassesFromLLM topicClassesFromLLM) : ControllerBase
    {
        private readonly SchoolRepository _schoolDb = schoolDb;
        private readonly TopicClassesFromLLM _topicClassesFromLLM = topicClassesFromLLM;

        [HttpPost("GetSchoolTopicsAuthors")]
        public async Task<ActionResult<string>> GetSchoolTopicsAuthors([FromBody] string schoolName, int subTopicsAmount)
        {
            try
            {
                var topicClassesAPIResponse = await _topicClassesFromLLM.GetNewTopicClassesFromLLM(schoolName, subTopicsAmount);
                var newTopicClasses = topicClassesAPIResponse.Value?.TopicClassesAquired;

                School school = new()
                {
                    Name = schoolName,
                    Topics = newTopicClasses!
                };

                var subTopicsResponse = await _topicClassesFromLLM.GetNewSubTopicClassesFromLLM(school, 10);
                await _schoolDb.CreateSchoolAsync(subTopicsResponse.Value!.School);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.ToString()}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
