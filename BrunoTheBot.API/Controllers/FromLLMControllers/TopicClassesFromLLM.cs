using BrunoTheBot.API.Controllers.FromLLMControllers;
using BrunoTheBot.API.Controllers.StaticsStatusCodes;
using BrunoTheBot.API.Prompts;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.CoreBusiness.Entities.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.FromLLMToDBControllers
{
    public class TopicClassesFromLLM(IChatGPTRequest chatGPTAPI, FromLLMToLogController fromLLMToLogController) : ControllerBase
    {
        private readonly IChatGPTRequest _chatGPTRequest = chatGPTAPI;
        private readonly FromLLMToLogController _fromLLMToLogController = fromLLMToLogController;

        public async Task<ActionResult<TopicClassesAPIResponse>> GetNewTopicClassesFromLLM(string school, int subTopicsClassesAmount = 10)
        {
            try
            {
                var prompt = LLMPrompts.GetTopicsClassesFromSchoolPrompt(school, subTopicsClassesAmount);
                var responseLLM = await _chatGPTRequest.ChatWithGPT(prompt) ?? throw new Exception();
                await _fromLLMToLogController.SaveLog(nameof(GetNewTopicClassesFromLLM), responseLLM);
                var newTopicClasses = ConvertJSONToObjects.ConvertToTopicClasses(responseLLM, "TopicClasses");
                if (newTopicClasses.Count <= 0 || newTopicClasses == null) throw new Exception("The TopicClassesResponseAPILLM amount is zero or null");

                TopicClassesAPIResponse TopicClassesResponseAPILLM = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    TopicClassesAquired = newTopicClasses
                };

                return TopicClassesResponseAPILLM ?? throw new Exception("TopicClassesResponseAPILLM is null");
            }
            catch (Exception ex)
            {
                var responseObject = new
                {
                    status = CustomStatusCodes.SuccessStatus,
                    response = ex.Message
                };

                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }

        public async Task<ActionResult<SchoolAPIResponse>> GetNewSubTopicClassesFromLLM(School school, int subTopicsClassesAmount = 10)
        {
            School updatedSchool = new School
            {
                Name = school.Name,
                Topics = new List<TopicClass>(school.Topics)
            };

            try                                 
            {
                foreach(var topic in school.Topics)
                {
                    var prompt = LLMPrompts.GetSubTopicsClassesFromSchoolPrompt(school.Name, topic.Name, subTopicsClassesAmount);
                    var responseLLM = await _chatGPTRequest.ChatWithGPT(prompt);
                    await _fromLLMToLogController.SaveLog(nameof(GetNewSubTopicClassesFromLLM), responseLLM);
                    var subTopicClasses = ConvertJSONToObjects.ConvertToTopicClasses(responseLLM, "SubTopicClasses");
                    topic.SubTopicClasses.AddRange(subTopicClasses);
                    updatedSchool.Topics.Add(topic);
                }

                SchoolAPIResponse SchoolAPIResponse = new()
                {
                    Status = CustomStatusCodes.SuccessStatus,
                    School = school
                };

                return SchoolAPIResponse ?? throw new Exception();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
