using BrunoTheBot.CoreBusiness.Entities.Course;
using Newtonsoft.Json.Linq;

namespace BrunoTheBot.API.Services
{
    public static class ConvertJSONToObjects
    {
        public static List<TopicClass> ConvertToTopicClasses(string input, string key)
        {
            try
            {
                var content = ExtractChatGPTContentFromJSON(input);
                JObject contentData = JObject.Parse(content);
                var topicClassesArray = (JArray)contentData[key]! ?? throw new Exception("TopicClasses not found in JSON.");

                List<TopicClass> _topicClassesList = new(topicClassesArray.Count);

                foreach (var item in topicClassesArray)
                {
                    var topicClass = new TopicClass
                    {
                        Name = item.ToString()
                    };
                    _topicClassesList.Add(topicClass);
                }

                if (_topicClassesList.Count == 0 || _topicClassesList == null) throw new Exception();

                return _topicClassesList;
            }

            catch
            {
                throw new Exception();
            }
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