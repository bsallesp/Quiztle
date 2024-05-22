using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.API.Prompts
{
    public static class CreateQuestionsTokensPrompts
    {
        public static string GetQuestionsFromPartOfPDFString(string text, int questionsAmount = 5)
        {
            string output = "";

            string json = $@"
            {{
                ""Question"": ""Write a captivating and creative question related to {text}."",
                ""Answer"": ""Here is the right answer."",
                ""Option1"": ""Here is a plausible but incorrect answer that may seem correct at first."",
                ""Option2"": ""Here is another plausible but incorrect answer that may seem correct at first."",
                ""Option3"": ""Here is a misleading answer that seems related but is ultimately incorrect."",
                ""Option4"": ""Here is a deceptive answer that may lead the player astray."",
                ""Hint"": ""Write a hint here that guides the player without giving away the answer.""
            }}
            ";

            output = $"Craft {questionsAmount} captivating and creative questions about {text} and fit it in the JSON structure: {json}." +
                $" Remember, it's just a part of entire book. I will sent more parts. Avoid repetitive questions.";

            return output;
        }
    }
}