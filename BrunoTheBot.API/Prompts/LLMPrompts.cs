using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.DataContext.Migrations;

namespace BrunoTheBot.API.Prompts
{
    public static class LLMPrompts
    {
        public static string GetNewTopicsClassesFromSchoolPrompt(string school, int topicClassAmount = 10)
        {
            return $"\"Let's devise a focused study plan to comprehensively learn about {school}." +
                $" Return a JSON with the key 'NewTopicClasses'" +
                $" containing an array of the {topicClassAmount} most crucial components.";
        }

        public static string GetNewSubTopicsFromTopicClasses(string school, string topicClasses, int subTopicClassAmount = 10)
        {
            string json = $@"
            {{
                ""NewSubTopicClasses"": [ ""subtopic1"", ""subtopic2"", ... ]
            }}
            ";

            return $"Let's devise a part of focused study plan to comprehensively learn about {topicClasses}," +
                $" a topic class about {school}. Return a JSON with the key 'NewSubTopicClasses'," +
                $" about {topicClasses} containing an array of the {subTopicClassAmount} most crucial components." +
                $" The JSON structure must be: {json}. This JSON represents an object with a key 'NewSubTopicClasses'," +
                $" which contains an array of subtopics (subtopic1, subtopic2, etc.)." +
                $" Each element in the array represents a sub-topic class.";
        }

        public static string GetNewContentFromSubTopics(string school, string topicClass, string subTopic)
        {
            string json = $@"
            {{
                ""NewContent"": ""The content you need to create.
                It must be didactic and easy to understand,
                while also aligning with the highest standards of knowledge
                from the world's top universities
                and leading scientific authors and materials.""
            }}
            ";

            return $"Let's devise a focused study plan to comprehensively learn about {subTopic}," +
                   $" a subtopic within {topicClass}, which is part of the {school} curriculum." +
                   $" Your task is to create new content that is didactic and easy to understand," +
                   $" while also ensuring it meets the highest standards of knowledge from the world's top universities" +
                   $" and leading scientific authors and materials." +
                   $" Return a JSON object with the key 'NewContent', containing the following structure: {json}";
        }
    }
}