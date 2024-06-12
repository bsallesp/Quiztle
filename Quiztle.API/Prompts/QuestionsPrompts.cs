namespace Quiztle.API.Prompts
{
    public static class QuestionsPrompts
    {
        public static string GetNewQuestionFromPages(string text, int questionsAmount = 5)
        {
            string output = "";

            string json = $@"
            {{
                ""Questions""
                {{
                    ""Question"": ""Write a captivating and creative question related to {text}."",
                    ""Answer"": ""Here is the right answer. Answers and options lack any distinctive writing patterns,
                                ensuring that the correct answer doesn't stand out from the group."",
                    ""Option1"": ""Here is a plausible but incorrect answer that may seem correct at first."",
                    ""Option2"": ""Here is another plausible but incorrect answer that may seem correct at first."",
                    ""Option3"": ""Here is a misleading answer that seems related but is ultimately incorrect."",
                    ""Option4"": ""Here is a deceptive answer that may lead the player astray."",
                    ""Hint"": ""Write a hint here that guides the player without giving away the answer."",
                    ""Resolution"": ""Provide a complete and detailed resolution, mentioning the page where the question originated.""
                }}
                {{another question if more than one and so on....}}
            }}
            ";

            if (questionsAmount <= 1)
            {
                output = $"Craft a captivating and creative question about {text} and fit it in the JSON structure: {json}";
            }
            else
            {
                output = $"Craft the amount of {questionsAmount} captivating and creative questions about {text}" +
                    $" and fit them in the JSON structure: {json}";
            }

            return output;
        }
    }
}
