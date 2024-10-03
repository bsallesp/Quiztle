using Quiztle.CoreBusiness.Entities.Quiz;

namespace Quiztle.API.Prompts
{
    public static class CurationPrompt
    {
        public static string GeneratePrompt(Question question)
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
