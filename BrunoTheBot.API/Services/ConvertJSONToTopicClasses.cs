using BrunoTheBot.CoreBusiness.Entities.Course;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop.Implementation;
using Newtonsoft.Json.Linq;

namespace BrunoTheBot.API.Services
{
    public static class ConvertJSONToObjects
    {
        public static List<TopicClass> ConvertToTopicClasses(string input)
        {
            var content = ExtractChatGPTContentFromJSON(input);
            JObject contentData = JObject.Parse(content);
            var topicClassesArray = (JArray)contentData["TopicClasses"]! ?? throw new Exception("TopicClasses not found in JSON.");

            List<TopicClass> _topicClassesList = new List<TopicClass>(topicClassesArray.Count);

            foreach (var item in topicClassesArray)
            {
                var topicClass = new TopicClass();
                topicClass.Name = item.ToString();
                Console.WriteLine(item.ToString());
                _topicClassesList.Add(topicClass);
            }

            return _topicClassesList;
        }

        public static string ExtractChatGPTContentFromJSON(string input)
        {
            try
            {
                JObject jsonObject = JObject.Parse(input);

#pragma warning disable CS8600
                string content = (string)jsonObject["choices"]![0]!["message"]!["content"] ?? throw new Exception();
#pragma warning restore CS8600

                return content;
            }

            catch
            {
                throw new Exception();
            }
        }
    }
}