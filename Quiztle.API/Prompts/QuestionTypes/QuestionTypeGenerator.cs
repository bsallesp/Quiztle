namespace Quiztle.API.Prompts
{
    public static class QuestionTypeGenerator
    {
        private static readonly string[] QuestionTypes = {
        "Direct Factual Question: A direct question about a specific fact or piece of data.",
        "Definition Selection: A question asking for the definition of a particular term or concept.",
        "Function Selection: A question inquiring about the function of a concept or entity.",
        "Complete the Sentence: A question requiring the selection of the correct option to complete a sentence.",
        "Term Association: A question that asks to associate related terms or concepts.",
        "Cause Selection: A question identifying the cause of a phenomenon or event.",
        "Effect Selection: A question requesting identification of the effect of an action or event.",
        "Correct Sequence: A question regarding the correct order of steps in a process.",
        "Exclusion Question: A question identifying the item that does not belong to a specific group.",
        "Incorrect Option Selection: A question asking to identify the incorrect statement among given options.",
        "Exception Selection: A question requiring identification of an exception to a rule or trend.",
        "Inference Based on Information: A question asking for an inference based on provided information.",
        "Best Explanation Selection: A question requesting the most plausible explanation for a phenomenon.",
        "Problem Scenario: A question presenting a hypothetical scenario that requires resolution.",
        "Concept Comparison: A question requiring comparison between two or more concepts.",
        "True Statement Selection: A question asking to select the only true statement from a list.",
        "Abstract Conceptual Question: A question testing understanding of an abstract concept.",
        "Graph or Table Based Question: A question requiring interpretation of graphical or tabular data.",
        "Calculation Question: A question necessitating simple calculations.",
        "Temporal Sequence: A question about the chronological order of events.",
        "Improvement Selection: A question requesting a suggestion for improvement in a situation.",
        "Cause and Consequence: A question about the relationship between cause and effect.",
        "Realistic Scenario: A question based on a real-world scenario requiring knowledge application.",
        "Error Identification: A question presenting a statement with an error to identify.",
        "Characteristic Comparison: A question comparing characteristics between two or more items.",
        "Outcome Selection: A question predicting the result of an experiment or situation.",
        "Pattern Recognition: A question asking to recognize patterns in data.",
        "Process Selection: A question identifying the appropriate process in a given situation.",
        "Cause and Effect Analysis: A question analyzing the relationship between a cause and its effect.",
        "Social Impact Selection: A question identifying social impacts of a specific phenomenon."
    };

        private static readonly Random random = new Random();

        public static string GetRandomQuestionType()
        {
            int index = random.Next(QuestionTypes.Length);
            return QuestionTypes[index];
        }
    }
}
