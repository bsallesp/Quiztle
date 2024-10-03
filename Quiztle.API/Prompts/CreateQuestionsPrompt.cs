using System.Text;

namespace Quiztle.API.Prompts
{
    public static class CreateQuestionsPrompt
    {
        public static string GetNewQuestionFromPages(string bookArticle, IEnumerable<string>? questionsAlreadyMade = null, int questionsAmount = 5, int incorrectOptionsAmount = 4)
        {
            if (string.IsNullOrEmpty(bookArticle))
            {
                throw new ArgumentException("Article content cannot be null or empty.", nameof(bookArticle));
            }

            var promptBuilder = new StringBuilder();

            AddHeader(promptBuilder);
            AddArticleContent(promptBuilder, bookArticle);
            AddJsonStructure(promptBuilder, incorrectOptionsAmount);

            if (questionsAlreadyMade?.Any() == true)
            {
                AddPreviousQuestions(promptBuilder, questionsAlreadyMade);
            }

            AddQuestionDetails(promptBuilder, questionsAmount, incorrectOptionsAmount);

            return promptBuilder.ToString();
        }

        private static void AddHeader(StringBuilder promptBuilder)
        {
            promptBuilder.AppendLine("As a specialist in constructing exams for the Azure AZ-900 Fundamentals Certification, " +
                                     "create distinct and clear questions based strictly on the provided text. Avoid synonyms, rephrasing, or ambiguity in answer choices.");
            promptBuilder.Append(QuestionTypeGenerator.GetRandomQuestionType());
        }

        private static void AddArticleContent(StringBuilder promptBuilder, string bookArticle)
        {
            promptBuilder.AppendLine("----------------------BEGIN OF ARTICLE-----------------------------------------------------------");
            promptBuilder.AppendLine(bookArticle);
            promptBuilder.AppendLine("----------------------END OF ARTICLE-----------------------------------------------------------");
        }

        private static void AddJsonStructure(StringBuilder promptBuilder, int incorrectOptionsAmount)
        {
            promptBuilder.AppendLine("Provide questions and answers in the JSON structure below:");
            promptBuilder.AppendLine(GetJsonStructure(incorrectOptionsAmount));
            promptBuilder.AppendLine("Only use the information provided above. Do not add any content.");
            promptBuilder.AppendLine("Ensure answers are similar in length and style, and that incorrect answers are plausible but clearly distinguishable from the correct answer.");
        }

        private static void AddPreviousQuestions(StringBuilder promptBuilder, IEnumerable<string> questionsAlreadyMade)
        {
            promptBuilder.AppendLine("Avoid repeating or creating variations of the following questions:");
            foreach (var question in questionsAlreadyMade)
            {
                promptBuilder.AppendLine($"{question}");
            }
            promptBuilder.AppendLine("Ensure new questions have a different structure and context.");
        }

        private static void AddQuestionDetails(StringBuilder promptBuilder, int questionsAmount, int incorrectOptionsAmount)
        {
            promptBuilder.AppendLine($"Total questions: {questionsAmount}.");
            promptBuilder.AppendLine("Total Correct Answers: 1.");
            promptBuilder.AppendLine($"Total Incorrect Answers: {incorrectOptionsAmount}.");
            promptBuilder.AppendLine("Ensure questions and answers strictly align with the provided text.");
            promptBuilder.AppendLine("Make sure the hint provides a useful clue for arriving at the correct answer, but avoid making it too obvious.");
            promptBuilder.AppendLine("Ensure the explanation is at least 50 words long, providing detailed reasoning to justify why the given answer is correct.");
            promptBuilder.AppendLine("When creating incorrect options, ensure they are plausible but have clear differences that highlight why they are incorrect compared to the correct answer.");
            promptBuilder.AppendLine("In Rate field, rate it form 0 to 5 (where 0 the resolution is totally easy, 5 is completely hard)");
        }

        private static string GetJsonStructure(int incorrectOptionsAmount)
        {
            return $@"{{
                ""Questions"": [
                    {{
                        ""Name"": ""<Question Text>"",
                        ""Options"": [
                            {{ ""Name"": ""<Correct Option>"", ""IsCorrect"": true }},
                            {{ ""Name"": ""<Distinct Incorrect Option 1>"", ""IsCorrect"": false }},
                            {{ ""Name"": ""<Distinct Incorrect Option 2>"", ""IsCorrect"": false }},
                            {{ ""Name"": ""<Distinct Incorrect Option 3>"", ""IsCorrect"": false }}
                        ],
                        ""Hint"": ""<Hint Text>"",
                        ""Resolution"": ""<Resolution Text>"",
                        ""Rate"": ""Rate integer""
                    }}
                ]
            }}";
        }
    }
}
