using Newtonsoft.Json.Linq;
using BrunoTheBot.CoreBusiness.Entities.Quiz;

namespace BrunoTheBot.APIs.Service
{
    public static class ChatGPTContentJSONParse
    {
        public static string MainParse(string json = "")
        {
            try
            {
                Console.WriteLine("Retrieving...");
                Console.WriteLine(json);

                if (!string.IsNullOrEmpty(json))
                {
                    JObject jsonObject = JObject.Parse(json);
#pragma warning disable CS8600
                    json = (string)jsonObject["choices"]?[0]?["message"]?["content"] ?? "";
#pragma warning restore CS8600
                }

                else
                {
                    Console.WriteLine("Input JSON is empty or null.");
                }

                return json;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                return json;
            }
        }

        public static School PopulateSchool(string content)
        {
            // Parse the JSON content
            var jsonObject = JObject.Parse(content);

            // Extract the topic name and subtopics
#pragma warning disable CS8600
            var topicName = (string)jsonObject["topic"];
#pragma warning restore CS8600

            var subtopics = jsonObject["subtopics"]?.ToObject<List<string>>();

            // Create a new school
            var school = new School
            {
                Name = topicName ?? "",
                Created = DateTime.Now,
                Topics = new List<Topic>()
            };

            // Populate topics with subtopics
            if (subtopics != null)
            {
                foreach (var subtopic in subtopics)
                {
                    school.Topics.Add(new Topic
                    {
                        Name = subtopic,
                        Created = DateTime.Now
                    });
                }
            }

            return school;
        }
    }
}
