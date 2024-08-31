namespace Quiztle.API.Prompts
{
    public static class QuestionsPrompts
    {
        public static string GetNewQuestionFromPages(string text, int questionsAmount = 5)
        {
            string description = $@"

                1. **`Questions`**: 
                   - Type: Array of objects.
                   - Description: A list of questions. Each object in the array represents a specific question.

                2. **Each object within the `Questions` array**:
                   - Type: Object.
                   - Description: Represents a specific question.

                   - **`Name`**:
                     - Type: String.
                     - Description: The text of the question. The question formulated about the provided text.

                   - **`Options`**:
                     - Type: Array of objects.
                     - Description: A list of answer options for the question. Each object in the array represents a possible answer.

                     - **Each object within the `Options` array**:
                       - Type: Object.
                       - Description: Represents a possible answer option.

                       - **`Name`**:
                         - Type: String.
                         - Description: The text of the answer option. The potential answer to the question.

                       - **`IsCorrect`**:
                         - Type: Boolean.
                         - Description: Indicates whether the option is the correct answer (`true`) or not (`false`). Only one option should be marked as correct (`true`).

                   - **`Hint`**:
                     - Type: String.
                     - Description: A hint to assist in answering the question. Should guide the respondent without directly revealing the correct answer.

                   - **`Resolution`**:
                     - Type: String.
                     - Description: A complete explanation of the correct answer. Should justify why the answer is correct and may mention the source of the question (e.g., the page of a document).
            ";

            if (questionsAmount <= 1)
            {
                return $"Create a question about '{text}' and format the JSON according to the provided description: {description}. The json ras";
            }
            else
            {
                return $"Create {questionsAmount} questions about '{text}' and format each one according to the described JSON structure: {description}";
            }
        }
    }
}


//namespace Quiztle.API.Prompts
//{
//    public static class QuestionsPrompts
//    {
//        public static string GetNewQuestionFromPages(string text, int questionsAmount = 5)
//        {
//            string output = "";

//            string json = $@"
//            {{
//                ""QuestionsDTO""
//                {{
//                    ""Question"": ""Write a captivating and creative question related to {text}."",
//                    ""Answer"": ""Here is the right answer. Answers and options lack any distinctive writing patterns,
//                                ensuring that the correct answer doesn't stand out from the group."",
//                    ""Option1"": ""Here is a plausible but incorrect answer that may seem correct at first."",
//                    ""Option2"": ""Here is another plausible but incorrect answer that may seem correct at first."",
//                    ""Option3"": ""Here is a misleading answer that seems related but is ultimately incorrect."",
//                    ""Option4"": ""Here is a deceptive answer that may lead the player astray."",
//                    ""Hint"": ""Write a hint here that guides the player without giving away the answer."",
//                    ""Resolution"": ""Provide a complete and detailed resolution, mentioning the page where the question originated.""
//                }}
//                {{another question if more than one and so on....}}
//            }}
//            ";

//            if (questionsAmount <= 1)
//            {
//                output = $"Craft a captivating and creative question about {text} and fit it in the JSON structure: {json}";
//            }
//            else
//            {
//                output = $"Craft the amount of {questionsAmount} captivating and creative questions about {text}" +
//                    $" and fit them in the JSON structure: {json}";
//            }

//            return output;
//        }
//    }
//}
