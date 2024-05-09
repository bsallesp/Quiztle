using BrunoTheBot.CoreBusiness.Entities.Course;

namespace BrunoTheBot.API.Prompts
{
    public static class LLMPrompts
    {
        public static string GetNewChaptersFromBookPrompt(string book, int chaptersAmount = 10)
        {
            string json = $@"
            {{
                ""NewChapters"": [ ""Chapter 1"", ""Chapter 2"", ... ]
            }}
            ";

            return $"We're writing a book titled '{book}'." +
                $" The structure is: a book has many chapters that have many sections." +
                $" Your mission now is to write {chaptersAmount} chapters about the book '{book}'." +
                $" Please return a JSON with the key 'NewChapters' in this format: {json}," +
                $" containing a list of the selected chapters.";
        }

        public static string GetNewSectionsFromChapters(Book book, int sectionsAmount = 10)
        {
            string json = $@"
            {{
                ""NewSections"": [ ""Section 1"", ""Section 2"", ... ]
            }}
            ";

            return $"We're writing a book titled '{book.Name}'." +
                $" The structure is: a book has many chapters that have many sections." +
                $" Your mission now is to write {sectionsAmount} sections about '{book.Chapters[0].Name}', which is a chapter of the book '{book.Name}'." +
                $" Please provide smart and intuitive sections about '{book.Chapters[0].Name}' of '{book.Name}'." +
                $" Please return a JSON with the key 'NewSections' in this format: {json},";
        }


        public static string GetNewContentFromSection(string bookName, string chapter, string section)
        {
            string json = $@"
            {{
                ""NewContent"": ""Here you answer the question in a didactic style, with details.""
            }}
            ";

            return $"We're writing a book titled '{bookName}'." +
                $" The structure is: a book has many chapters that have many sections that have a body." +
                $" Your mission now is to write a body about '{section}'," +
                $" which is a section of the chapter {chapter}," +
                $" which is a chapter of the '{bookName}'." +
                $" Please provide smart and intuitive content." +
                $" Please return a JSON with the key 'NewContent' in this format: {json},";
        }

        public static string GetNewQuestion(string text, int questionsAmount = 5)
        {
            string output = "";

            string json =  $@"
            {{
                ""Question"": ""Write the question here."",
                ""Answer"": ""Here is the right answer."",
                ""Option1"": ""Here is a wrong answer."",
                ""Option2"": ""Here is a wrong answer."",
                ""Option3"": ""Here is a wrong answer."",
                ""Option4"": ""Here is a wrong answer."",
                ""Hint"": ""Write a hint here.""
            }}
            ";

            if (questionsAmount <= 1) output = $"Elaborate a question about {text} and fit it in the json structure: {json}";
            if (questionsAmount > 1) output = $"Elaborate {questionsAmount} questions about {text} and fit it in the json structure: {json}";

            return output; 
        }
    }
}