namespace BrunoTheBot.API.Prompts
{
    public static class QuestionsPrompts
    {
        public static string GetQuestionsFromPages(List<string> texts, int startPage, int endPage, int questionsAmount = 5)
        {
            string output = "Below are " + questionsAmount + " captivating and creative questions based on the content extracted from the PDF." +
                " Use these questions to test your understanding:";

            output += "\n[\n";

            for (int i = 0; i < questionsAmount; i++)
            {
                int selectedPageIndex = i % texts.Count; 
                string selectedText = texts[selectedPageIndex];
                int currentPage = startPage + selectedPageIndex;

                output += $@"
                {{
                    ""Id"": {i + 1},
                    ""Name"": ""Question {i + 1}"",
                    ""Question"": ""Write a captivating and creative question related to {selectedText}"",
                    ""Answer"": ""Here is the right answer."",
                    ""Options"": [
                        {{""Id"": 1, ""Name"": ""Here is a plausible but incorrect answer that may seem correct at first.""}},
                        {{""Id"": 2, ""Name"": ""Here is another plausible but incorrect answer that may seem correct at first.""}},
                        {{""Id"": 3, ""Name"": ""Here is a misleading answer that seems related but is ultimately incorrect.""}},
                        {{""Id"": 4, ""Name"": ""Here is a deceptive answer that may lead the player astray.""}}
                    ],
                    ""Hint"": ""This question is based on text from page {currentPage}. To understand this, refer back to the content on this page for the correct context and explanation.""
                }}";

                if (i < questionsAmount - 1)
                    output += ",\n";  // Add comma to separate questions
            }

            output += "\n]"; // End of JSON list

            output += @"
            \nEnsure that the JSON structure corresponds with the C# entity definitions provided for compatibility with the system's data model.";

            // Reminder of the C# entity structure (optional here for clarity)
            output += @"
            \n[JsonPropertyName(""Name"")]
            public string Name { get; set; } = "";

            [JsonPropertyName(""Answer"")]
            public string? Answer { get; set; } = "";

            [JsonPropertyName(""Options"")]
            public List<Option> Options { get; set; } = new List<Option>();

            [JsonPropertyName(""Hint"")]
            public string? Hint { get; set; } = "";

            public class Option
            {
                [JsonPropertyName(""Name"")]
                public string Name { get; set; } = "";
            }
            ";

            return output;
        }
    }
}
