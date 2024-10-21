using Quiztle.CoreBusiness.Entities.Quiz;
using System.Text;

namespace Quiztle.API.Prompts
{
    public static class CurationPrompt
    {
        public static string GenerateScorePrompt(Question question)
        {
            var result = $@"
                I will provide you with a question and answers intended to help study for the AZ-900 Microsoft Azure Fundamentals exam.
                Your task is to evaluate the overall quality of this quiz based on the following criteria:

                - Relevance to the AZ-900 exam content
                - Clarity and accuracy of the questions and answers
                - Level of difficulty appropriate for someone studying for the AZ-900 exam
                - Consistency of the Question and options

                Please provide a score from 0 to 5 based on the quality of the quiz, where:

                SCORE:
                - A score of 0 indicates that the question should be excluded from the exam (the question is nonsensical or does not aid learning).
                - A score of 1 indicates a very easy question.
                - A score of 2 indicates an easy question.
                - A score of 3 indicates a medium difficulty question.
                - A score of 4 indicates a difficult question.
                - A score of 5 indicates a very difficult question.

                CONSISTENCY:
                - A Consistency of 0 indicates that the question is inconsistent and not suitable for a professional exam.
                - A Consistency of 1 indicates that the question is consistent and suitable for a professional exam.

                Here is the content from which the question was derived:
                {question.Draft!.Text}

                Here is the quiz for evaluation:
                {QuestionString(question)}

                Return the evaluation in the following JSON format:

                {{
                  ""score"": [Your score from 0 to 5],
                  ""consistency"": [Your score from 0 to 1]
                }}
            ";

            Console.WriteLine(result);
            
            return result;
        }


        public static string GenerateAnswerValidationPrompt(IEnumerable<Question> questions)
        {
            var result = new StringBuilder();

            // Introduction
            result.AppendLine("I will provide you with a list of questions and their corresponding answers.");
            result.AppendLine("Your task is to confirm the correctness of each answer.");
            result.AppendLine();
            result.AppendLine("Here are the quizzes for your evaluation:");

            // List questions and options
            foreach (var question in questions)
            {
                result.AppendLine($"{question.Id}: {question.Name}");
                foreach (var option in question.Options)
                {
                    result.AppendLine($"- {option.Name} (Correct: {option.IsCorrect})");
                }
                result.AppendLine(); // Add a newline for separation
            }

            // Instructions for JSON format
            result.AppendLine("For each question, confirm the correctness of the options in the following JSON format:");

            // JSON format template
            var jsonFormat = new StringBuilder();
            jsonFormat.AppendLine("{");

            foreach (var question in questions)
            {
                jsonFormat.AppendLine($"  \"{question.Id}\": {{");
                jsonFormat.AppendLine("    \"OptionsDTO\": [");

                foreach (var option in question.Options)
                {
                    jsonFormat.AppendLine($"      {{ \"Id\": \"{option.Id}\", \"IsCorrect\": true | false }} // {option.Name} (Indicate true or false)");
                }

                jsonFormat.AppendLine("    ]");
                jsonFormat.AppendLine("  },");
            }

            // Remove the last comma and space
            if (questions.Any())
            {
                jsonFormat.Length -= 3; // Remove the last comma and space
            }

            jsonFormat.AppendLine("}");

            result.AppendLine(jsonFormat.ToString());

            return result.ToString();
        }

        public static string AIPlayQuestionsPrompt(string draft, Question question)
        {
            var result = new StringBuilder();

            //result.AppendLine("Read the text below: ");

            //result.AppendLine(draft);

            result.AppendLine("Read the question and answer: Whats the correct option? I just need to now the guid that belongs the correct option:");

            result.AppendLine(question.Name);
            foreach(var option in question.Options)
            {
                result.AppendLine("Option guid: " + option.Id + " - " + option.Name);
            }

            result.AppendLine("Return ony the guid of the correct option. Dont return anything else.");
            result.AppendLine("Totally ignore incorrect Options");

            return result.ToString();
        }

        private static string QuestionString(Question question)
        {
            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();
            var result = $"Question: {question.Name}\n"; // Use string interpolation for clarity
            var i = 0;

            foreach (var option in question.Options)
            {
                result += $"({alphabet[i++]}) {option.Name} ({(option.IsCorrect ? "marked as correct" : "marked as incorrect")})\n";
            }

            return result;
        }
    }
}
