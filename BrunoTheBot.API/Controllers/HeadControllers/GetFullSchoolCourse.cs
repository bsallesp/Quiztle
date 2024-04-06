using BrunoTheBot.API.Controllers.FromLLMToDBControllers;
using BrunoTheBot.API.Services;
using BrunoTheBot.CoreBusiness.APIEntities;
using BrunoTheBot.DataContext.DataService.Repository.Course;
using Microsoft.AspNetCore.Mvc;

namespace BrunoTheBot.API.Controllers.HeadControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFullSchoolCourse(SchoolRepository schoolDb, TopicClassesFromLLM topicClassesFromLLM) : ControllerBase
    {
        private readonly SchoolRepository _schoolDb = schoolDb;
        private readonly TopicClassesFromLLM _topicClassesFromLLM = topicClassesFromLLM;

        [HttpPost("GetSchoolTopicsAuthors")]
        public async Task<ActionResult<string>> GetSchoolTopicsAuthors([FromBody] string input, int subTopicsAmount)
        {
            try
            {
                var topicClassesAPIResponse = await _topicClassesFromLLM.GetNewTopicClassesFromLLM(input, subTopicsAmount);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.ToString()}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }


        [HttpPost("TESTTEST")]
        public ActionResult<string> TESTTEST()
        {
            string test2 = @"

                    {
  ""id"": ""chatcmpl-9B1JRbS9dxayUazuRdJPXc4xJFgSw"",
  ""object"": ""chat.completion"",
  ""created"": 1712413681,
  ""model"": ""gpt-3.5-turbo-0125"",
  ""choices"": [
    {
      ""index"": 0,
      ""message"": {
        ""role"": ""assistant"",
        ""content"": ""{\n  \""TopicClasses\"": [\n    \""Docker Images\"",\n    \""Docker Containers\"",\n    \""Docker Compose\"",\n    \""Docker Volumes\"",\n    \""Docker Networking\"",\n    \""Docker Registries\"",\n    \""Dockerfile\"",\n    \""Docker Hub\"",\n    \""Docker Swarm\"",\n    \""Docker Security\""\n  ]\n}""
      },
      ""logprobs"": null,
      ""finish_reason"": ""stop""
    }
  ],
  ""usage"": {
    ""prompt_tokens"": 92,
    ""completion_tokens"": 74,
    ""total_tokens"": 166
  },
  ""system_fingerprint"": ""fp_b28b39ffa8""
}


                    ";



            string test = @"

                    {
                      ""id"": ""chatcmpl-9And4HHNzzXmLpiMI6payQ0uCnlQq"",
                      ""object"": ""chat.completion"",
                      ""created"": 1712361082,
                      ""model"": ""gpt-3.5-turbo-0125"",
                      ""choices"": [
                        {
                          ""index"": 0,
                          ""message"": {
                            ""role"": ""assistant"",
                            ""content"": ""{\n  \""TopicClasses\"": [\n    \""EC2 (Elastic Compute Cloud)\"",\n    \""S3 (Simple Storage Service)\"",\n    \""IAM (Identity and Access Management)\"",\n    \""RDS (Relational Database Service)\"",\n    \""VPC (Virtual Private Cloud)\"",\n    \""Lambda\"",\n    \""Route 53\"",\n    \""CloudFormation\"",\n    \""SNS (Simple Notification Service)\"",\n    \""CloudWatch\""\n  ]\n}""
                          },
                          ""logprobs"": null,
                          ""finish_reason"": ""stop""
                        }
                      ],
                      ""usage"": {
                        ""prompt_tokens"": 92,
                        ""completion_tokens"": 86,
                        ""total_tokens"": 178
                      },
                      ""system_fingerprint"": ""fp_b28b39ffa8""
                    }


                    ";

            try
            {
                var result = ConvertJSONToObjects.ConvertToTopicClasses(test2);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.GetType().Name} was thrown. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
            }
        }
    }
}
