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

        public static string GetNewSectionsFromTopicClasses(string school, string topicClass, int sectionsAmount = 10)
        {
            string json = $@"
            {{
                ""NewSections"": [ ""Question 1"", ""Question 2"", ... ]
            }}
            ";

            return $"I want to learn about charpter {topicClass}, of {school} Course, but I don't know even to ask a question about this." +
                       $" Please, generate the most smart and intuitive questions about {topicClass} of {school}." +
                       $" Generate the amount of {sectionsAmount} questions." +
                       $" The output JSON structure musto to be: {json}";
        }

        public static string GetNewContentFromSection(string schoolName, string topicClass, string section)
        {
            string json = $@"
            {{
                ""NewContent"": ""Here you answer the question in a didactic style, with details.""
            }}
            ";

            return $"I am studing about {schoolName}. Now I am the {topicClass} charpter. Can you answer this question? {section}" +
                $" Return a JSON object with the key 'NewContent'," +
                $" containing the following structure: {json}";
        }
    }
}