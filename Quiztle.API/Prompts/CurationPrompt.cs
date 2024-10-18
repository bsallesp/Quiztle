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
            result.AppendLine("I will provide you with a list of questions and their corresponding answers, with one or more correct answers.");
            result.AppendLine("Your task is to confirm which answers are correct.");
            result.AppendLine();
            result.AppendLine("Here are the quizzes for your evaluation:");

            // Build the question string
            foreach (var question in questions)
            {
                result.AppendLine($"{question.Id}: {question.Name}");
                foreach (var option in question.Options)
                {
                    result.AppendLine($"- {option.Name} (Correct: {option.IsCorrect})");
                }
                result.AppendLine(); // Add a newline for separation
            }

            result.AppendLine("If any answer marked as correct is actually incorrect, or if any answer marked as incorrect is actually correct, please indicate so.");
            result.AppendLine("If you agree with all the answers as demonstrated, return \"t\" (true). If any of the answers are wrong, return \"f\" (false).");
            result.AppendLine();
            result.AppendLine("Please confirm the correctness of the answers using the following format:");

            // Construct the JSON format using UUIDs as keys
            var jsonFormat = new StringBuilder();
            jsonFormat.AppendLine("{");
            foreach (var question in questions)
            {
                jsonFormat.AppendLine($"  \"{question.Id}\": t,  // answer is correct"); // Default to correct; adjust as necessary
            }
            jsonFormat.AppendLine("}");

            result.AppendLine(jsonFormat.ToString());

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
