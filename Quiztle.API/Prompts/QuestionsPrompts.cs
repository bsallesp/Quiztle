using Humanizer;
using System.Text;

namespace Quiztle.API.Prompts
{
    public static class QuestionsPrompts
    {
        public static string GetNewQuestionFromPages(string bookArticle, IEnumerable<string>? questionsAlreadyMade = null, int questionsAmount = 5, int incorrectOptionsAmount = 4)
        {
            if (string.IsNullOrEmpty(bookArticle))
            {
                throw new ArgumentException("Article content cannot be null or empty.", nameof(bookArticle));
            }

            var promptBuilder = new StringBuilder();

            // Adicionar cabeçalho
            AddHeader(promptBuilder);

            // Adicionar conteúdo do artigo
            AddArticleContent(promptBuilder, bookArticle);

            // Adicionar estrutura JSON
            AddJsonStructure(promptBuilder, incorrectOptionsAmount);

            // Adicionar perguntas anteriores se houver
            if (questionsAlreadyMade?.Any() == true)
            {
                AddPreviousQuestions(promptBuilder, questionsAlreadyMade);
            }

            // Configurar detalhes das perguntas
            AddQuestionDetails(promptBuilder, questionsAmount, incorrectOptionsAmount);

            //// Adicionar exemplo de pergunta
            //AddExampleQuestion(promptBuilder);

            return promptBuilder.ToString();
        }

        private static void AddHeader(StringBuilder promptBuilder)
        {
            promptBuilder.AppendLine("As a specialist in constructing exams for the Azure AZ-900 Fundamentals Certification, " +
                                     "create varied questions based strictly on the provided text. Avoid using synonyms or creatively rephrasing the content.");
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
            promptBuilder.AppendLine("Ensure answers are similar in length and style, and incorrect answers are plausible and detailed.");
        }

        private static void AddPreviousQuestions(StringBuilder promptBuilder, IEnumerable<string> questionsAlreadyMade)
        {
            promptBuilder.AppendLine("Avoid repeating or creating variations of the following questions:");
            foreach (var question in questionsAlreadyMade)
            {
                promptBuilder.AppendLine($"{question}");
            }
            promptBuilder.AppendLine("Ensure new questions differ in structure and context.");
        }

        private static void AddQuestionDetails(StringBuilder promptBuilder, int questionsAmount, int incorrectOptionsAmount)
        {
            promptBuilder.AppendLine($"Total questions: {questionsAmount}.");
            promptBuilder.AppendLine("Total Correct Answers: 1.");
            promptBuilder.AppendLine($"Total Incorrect Answers: {incorrectOptionsAmount}.");
            promptBuilder.AppendLine("Ensure questions and answers strictly align with the provided text.");
        }

        //private static void AddExampleQuestion(StringBuilder promptBuilder)
        //{
        //    promptBuilder.AppendLine("Here is an example of a question with proper structure and variation: ");
        //    promptBuilder.AppendLine(GetExampleQuestion());
        //}

        //private static string GetExampleQuestion()
        //{
        //    return "{\r\n  \"Questions\": [\r\n    {\r\n      \"Name\": \"Which of the following is a cost-saving feature of cloud computing?\",\r\n      \"Options\": [\r\n        {\r\n          \"Name\": \"You only pay for what you use.\",\r\n          \"IsCorrect\": true\r\n        },\r\n        {\r\n          \"Name\": \"Cloud providers offer free hardware.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"Cloud services do not incur bandwidth costs.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"Cloud services are only available in one region.\",\r\n          \"IsCorrect\": false\r\n        }\r\n      ],\r\n      \"Hint\": \"Consider how cloud providers bill for services.\",\r\n      \"Resolution\": \"The correct answer is 'You only pay for what you use'.\" \r\n    }\r\n  ]\r\n}";
        //}

        private static string GetJsonStructure(int incorrectOptionsAmount)
        {
            return $@"{{
                ""Questions"": [
                    {{
                        ""Name"": ""<Question Text>"",
                        ""Options"": [
                            {{ ""Name"": ""<Option 1>"", ""IsCorrect"": true }},
                            {{ ""Name"": ""<Option 2>"", ""IsCorrect"": false }},
                            {{ ""Name"": ""<Option 3>"", ""IsCorrect"": false }},
                            {{ ""Name"": ""<Option 4>"", ""IsCorrect"": false }}
                        ],
                        ""Hint"": ""<Hint Text>"",
                        ""Resolution"": ""<Resolution Text>""
                    }}
                ]
            }}";
        }
    }
}
