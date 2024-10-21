using System.Text;

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
            promptBuilder.AppendLine($"I have some content." +
                $" I need you to read it and create a title and a new summary of up to {lenght} characters about it:");

        }

        private static void AddOriginalContent(StringBuilder promptBuilder, string bookArticle)
        {
            promptBuilder.AppendLine("----------------------BEGIN OF ARTICLE-----------------------------------------------------------");
            promptBuilder.AppendLine(bookArticle);
            promptBuilder.AppendLine("----------------------END OF ARTICLE-----------------------------------------------------------");
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
                    ""Title"": ""<Create a nice title for the current draft>"",
                    ""MadeByAiContent"": ""<create a didactic and reliable summary here. Avoid the ""crucial"" word.>"",
                    ""Tag"": ""<Create a tag, like a short Title. Try to make beetween from 3 to 5 words. As shorter as better.>"",
                }}
            }}";
        }
    }
}
