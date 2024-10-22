using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Quiztle.API.Prompts
{
    public static class UpdateDraftPromt
    {
        public static string GetPromptString(string originalContent, int length)
        {
            if (string.IsNullOrEmpty(originalContent))
            {
                throw new ArgumentException("Article content cannot be null or empty.", nameof(originalContent));
            }

            var promptBuilder = new StringBuilder();

            AddHeader(promptBuilder, length);
            AddOriginalContent(promptBuilder, originalContent);
            AddSummaryInstruction(promptBuilder);
            AddJsonDraftStructure(promptBuilder);

            return promptBuilder.ToString();
        }

        private static void AddHeader(StringBuilder promptBuilder, int lenght)
        {
            promptBuilder.AppendLine(
                $"Imagine a meticulous and engaging professor, Dr. Elena Silva, known for her clarity and passion in teaching complex concepts. " +
                $"She approaches every lesson with enthusiasm and encourages her students to explore and question the material deeply. " +
                $"Dr. Silva uses relatable examples and visual aids to enhance understanding, making even the most abstract ideas accessible. " +
                $"Now, perform a technical analysis of the following text, explicitly focusing on the identification and exposition of key concepts. " +
                $"This is not a summary or narrative; instead, provide clear and precise definitions for each identified concept, " +
                $"utilizing relevant theoretical frameworks. Discuss the implications of these concepts within the context" +
                $" of the text and provide practical examples that demonstrate their application. " +
                $"Use appropriate terminology from the fields of linguistics, semantics, or discourse analysis. " +
                $"Generate it, up to {lenght} characters. This is the text: "
            );
        }

        private static void AddOriginalContent(StringBuilder promptBuilder, string bookArticle)
        {
            promptBuilder.AppendLine("----------------------BEGIN OF text-----------------------------------------------------------");
            promptBuilder.AppendLine(bookArticle);
            promptBuilder.AppendLine("----------------------END OF text-----------------------------------------------------------");
        }

        private static void AddSummaryInstruction(StringBuilder promptBuilder)
        {
            promptBuilder.AppendLine("Before generating the questions, summarize the key points from the article. " +
            "Identify crucial facts or concepts that can serve as the basis for your questions.");
        }

        private static void AddJsonDraftStructure(StringBuilder promptBuilder)
        {
            promptBuilder.AppendLine("Please provide the new draft summary in the JSON structure outlined below:");
            promptBuilder.AppendLine(GetJsonDraftStructure());
            promptBuilder.AppendLine("Ensure that you strictly adhere to the provided structure and only use the information given above. Do not add any additional content.");
            promptBuilder.AppendLine("The draft summary should be clear, reliable, and educational, effectively summarizing the key points of the original article.");
        }

        private static string GetJsonDraftStructure()
        {
            // Aqui estamos apenas definindo a estrutura JSON para o draft
            return $@"{{
                ""Draft"": {{

                    ""Title"": ""<Create a oficial title for the current draft>"",

                    ""MadeByAiContent"": ""<Imagine questions and answer them. Dont expose the questions, just answer them.
                        Create real life examples. Avoid the words like ""crucial"" and another cliche words.
                            Be didactic, Make ADHD people understand your explanation.>"",

                    ""Tag"""": """"<Create a concise, content-specific tag using 1 to 3 words that
                        directly represent the key topic or idea from the article.
                            Absolutely dont use common nouns. But use proper nouns. Avoid vague or general terms.>"""",
                }}
            }}";
        }
    }
}
