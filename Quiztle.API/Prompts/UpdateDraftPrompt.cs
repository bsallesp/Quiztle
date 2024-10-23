using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Quiztle.API.Prompts
{
    public static class UpdateDraftPromt
    {
        public static string GetPromptString(string originalContent, string[] tags)
        {
            if (string.IsNullOrEmpty(originalContent))
            {
                throw new ArgumentException("Text content cannot be null or empty.", nameof(originalContent));
            }

            var promptBuilder = new StringBuilder();

            AddHeader(promptBuilder);
            AddOriginalContent(promptBuilder, originalContent);
            AddHeaderJsonDraftStructure(promptBuilder, tags);

            return promptBuilder.ToString();
        }

        private static void AddHeader(StringBuilder promptBuilder)
        {
            promptBuilder.AppendLine(
                $"You will read a text bellow: "
            );
        }

        private static void AddOriginalContent(StringBuilder promptBuilder, string bookArticle)
        {
            promptBuilder.AppendLine("-----BEGIN OF text-------");
            promptBuilder.AppendLine(bookArticle);
            promptBuilder.AppendLine("-------END OF text-------");
        }

        private static void AddHeaderJsonDraftStructure(StringBuilder promptBuilder, string[] tags)
        {
            promptBuilder.AppendLine("in 'MadeByAiContent' value, do this: ");
            promptBuilder.AppendLine("Paraphrase: Rewrite the text in different words while maintaining the original meaning.\r\n" +
                "Examples and Illustrations: Add specific examples or analogies to clarify complex ideas.\r\n" +
                "Definitions and Explanations: Define key terms and concepts clearly, especially those that may be technical or specialized.\r\n" +
                "Sentence Structure Variation: Use a mix of simple, compound, and complex sentences to enhance readability and maintain interest.\r\n" +
                "Cohesion and Coherence Techniques: Ensure logical flow between ideas by using linking words and phrases\r\n" +
                "(e.g., however, furthermore, in addition).\r\n" +
                "Contextualization: Place the information within a broader context to highlight its significance and application.\r\n" +
                "Visual Aids: Suggest any relevant diagrams, charts, or illustrations that could complement the text\r\n" +
                "(you won’t create these, but mention them).\r\n" +
                "Summaries and Conclusions: Provide a brief summary or conclusion that reinforces the key points and solidifies understanding");

            promptBuilder.AppendLine("Dont speak in indicative mode. Dont speak about the text. Create your own text, reading this text.");
            promptBuilder.AppendLine("in 'Tag' Choose the best one of them.");
            promptBuilder.AppendLine("Ensure that you strictly adhere to the provided structure and only use the information given above.");
            promptBuilder.AppendLine(GetJsonDraftStructure(tags));
        }

        private static string GetJsonDraftStructure(string[] tags)
        {
            var tagsString = string.Join(", ", tags.Select(tag => $"\"{tag}\""));

            return $@"{{
            ""Draft"":
                {{

                    ""MadeByAiContent"": ""<The content here.>"",

                    ""Tag"": ""<Choose one of them: {tagsString}. Don't choose anything different.
                        Return the literal, one of them. Dont create another kind of tag.>""
                }}
            }}";
        }
    }
}
