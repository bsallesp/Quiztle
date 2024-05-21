using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.API.Prompts
{
    public static class CreateBookPrompts
    {
        public static string GetNewChaptersFromBookPrompt(string book, int chaptersAmount = 10)
        {
            string jsonExample = @"
    {
        ""NewChapters"": [ ""Chapter 1"", ""Chapter 2"", ... ]
    }";

            return $"We aim for a professional approach to '{book}'. " +
                   $"Please provide {chaptersAmount} chapters on '{book}', drawing inspiration from renowned academic sources " +
                   $"and esteemed authors who have contributed to the rich legacy of {book}. " +
                   $"Sort the chapters in ascending order. Provide only the text, omitting chapter numbers (e.g., 'Chapter 1', 'Chapter 2'). " +
                   $"Please return a JSON object with the key 'NewChapters' in the following format: {jsonExample}, " +
                   $"containing a list of the selected chapters.";
        }

        public static string GetNewSectionsFromChapters(Book book, string chapterName, int sectionsAmount = 10)
        {
            string json = $@"
            {{
                ""NewSections"": [ ""Section 1"", ""Section 2"", ... ]
            }}
            ";

            return $"We are committed to a professional approach in exploring '{book.Name}'." +
                    $" To comprehensively cover the concept of '{chapterName}', what are the key topics to include?" +
                    $" Provide {sectionsAmount} insightful sections pertaining to '{book.Chapters[0].Name}' from '{book.Name}'." +
                    $" Sort the sections in ascending order. Provide only the text, omitting section numbers (e.g., 'section 1', 'section 2'). " +
                    $" Return a JSON with the key 'NewSections' in the following format: {json}, in a total of '{sectionsAmount}'.";
        }

        public static string GetNewContentFromSection(string bookName, string chapter, string section)
        {
            string json = $@"
    {{
        ""NewContent"": ""Provide a comprehensive explanation in a didactic style, covering all key concepts and details.""
    }}
    ";

            return $"Provide a comprehensive explanation of '{section} - {chapter} - {bookName} - ," +
                $" focusing on key concepts and details.\r\n" +
                $" Avoid starting the text directly with the title to prevent repetitive beginnings." +
                $" Instead, aim for a clear and engaging introduction to capture the reader's attention.\r\n" +
                $" Include code snippets samples ONLY and ONLY if the topic pertains to programming.\r\n" +
                $" Once code created, be sure it has within '#BEGINCODE#' and '#ENDCODE#' tags.\r\n" +
                $" Consider integrating questions and answers to foster engagement throughout the content.\r\n" +
                $" Verify that the content is clear, detailed, and educational.\r\n" +
                $" If feasible, seek inspiration from top writers and universities on the subject.\r\n" +
                $" Avoid introductory narratives and dive straight into concept explanation.\r\n" +
                $" Return a JSON object with the key 'NewContent' formatted as follows: {json}.";
        }

        public static string GetNewQuestionFromLLMBook(string text, int questionsAmount = 5)
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

            if (questionsAmount <= 1)
            {
                output = $"Craft a captivating and creative question about {text} and fit it in the JSON structure: {json}";
            }
            else
            {
                output = $"Craft {questionsAmount} captivating and creative questions about {text} and fit them in the JSON structure: {json}";
            }

            return output;
        }
    }
}