using Newtonsoft.Json.Linq;
using BrunoTheBot.CoreBusiness.Entities;
using System;
using System.Collections.Generic;
using BrunoTheBot.CoreBusiness;

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
                    json = (string)jsonObject["choices"]?[0]?["message"]?["content"] ?? "";
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
            var topicName = (string)jsonObject["topic"];
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
