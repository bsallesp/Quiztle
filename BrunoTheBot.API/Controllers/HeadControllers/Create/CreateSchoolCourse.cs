using BrunoTheBot.API.Controllers.FromLLMToDBControllers;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;
using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.APIEntities;


namespace BrunoTheBot.API.Controllers.HeadControllers.Create
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateSchoolCourse(SchoolRepository schoolDb, TopicClassesFromLLM topicClassesFromLLM) : ControllerBase
    {
        private readonly SchoolRepository _schoolDb = schoolDb;
        private readonly TopicClassesFromLLM _topicClassesFromLLM = topicClassesFromLLM;

        [HttpPost("CreateSchoolFullCourse")]
        public async Task<ActionResult<SchoolAPIResponse>> ExecuteAsync([FromBody] string schoolName, int subTopicsAmount = 10)
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

                var schoolAPIResponse = await _topicClassesFromLLM.GetNewSubTopicClassesFromLLM(school, 10);
                var newSchoolWithTopicsAndSubTopics = schoolAPIResponse.Value!.School;
                await _schoolDb.CreateSchoolAsync(newSchoolWithTopicsAndSubTopics);

                return schoolAPIResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.ToString()}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
