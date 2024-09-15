using Humanizer;
using System.Text;

namespace Quiztle.API.Prompts
{
    public static class QuestionsPrompts
    {
        public static string GetNewQuestionFromPages(string bookPart, int questionsAmount = 5, int incorrectOptionsAmount = 4)
        {
            List<string> prompt = new();

            prompt.Add("Your objective, as a language teacher and specialist in constructing exams for technical certifications," +
                " is to build questions based exclusively on the words and phrases in the text. " +
                "Avoid using synonyms or creatively rephrasing the content.");

            prompt.Add("Observe this example:" +
                "Why is cloud computing often less expensive than on-premises datacenters?" +
                "Cloud service offerings have limited functionality." +
                "Network bandwidth is free." +
                "Services are only offered in a single geographic location." +
                "You are only billed for what you use." +
                "");

            prompt.Add("You must create questions and answers based on the content below:");

            prompt.Add(bookPart);

            prompt.Add("Provide questions and answers in the JSON structure below:");

            prompt.Add(GetJsonStructure(incorrectOptionsAmount));

            prompt.Add("Only use the information provided here. Do not add any content.");

            prompt.Add("You have already create similar questions, have a look");

            prompt.Add("You must to vary the questions you will make now.");

            prompt.Add("The student must be able to obtain the correct answer by reading the text provided above.");

            prompt.Add("Adopt a style similar to official certification exams.");

            prompt.Add("Total questions: " + questionsAmount + ".");
            prompt.Add("Total Correct Answers: 1.");
            prompt.Add("Total Incorrect Answers: " + incorrectOptionsAmount + ".");
            prompt.Add("Additional Note" +
                "\": \"Ensure that the questions and answers are strictly aligned with the provided text," +
                " without introducing new information or interpretations.");

            prompt.Add("Have a look in this example, with a only one question: " +
                "{\r\n  \"Questions\": [\r\n    {\r\n      \"Name\": \"Why is cloud computing often less expensive than on-premises datacenters?\",\r\n      \"Options\": [\r\n        {\r\n          \"Name\": \"Cloud service offerings have limited functionality.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"Network bandwidth is free.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"Services are only offered in a single geographic location.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"You are only billed for what you use.\",\r\n          \"IsCorrect\": true\r\n        },\r\n        {\r\n          \"Name\": \"Cloud providers use less expensive hardware.\",\r\n          \"IsCorrect\": false\r\n        }\r\n      ],\r\n      \"Hint\": \"Consider the billing model of cloud services compared to traditional datacenters.\",\r\n      \"Resolution\": \"Cloud computing is often less expensive than on-premises datacenters because you are only billed for what you use. This pay-as-you-go model allows organizations to scale resources up or down based on demand, reducing unnecessary costs.\"\r\n    }\r\n  ]\r\n}\r\n" +
                "");

            prompt.Add("Have a look in this example, with multiple questions: " +
            "{\r\n  \"Questions\": [\r\n    {\r\n      \"Name\": \"Why is cloud computing often less expensive than on-premises datacenters?\",\r\n      \"Options\": [\r\n        {\r\n          \"Name\": \"Cloud service offerings have limited functionality.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"Network bandwidth is free.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"Services are only offered in a single geographic location.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"You are only billed for what you use.\",\r\n          \"IsCorrect\": true\r\n        },\r\n        {\r\n          \"Name\": \"Cloud providers use less expensive hardware.\",\r\n          \"IsCorrect\": false\r\n        }\r\n      ],\r\n      \"Hint\": \"Consider the billing model of cloud services compared to traditional datacenters.\",\r\n      \"Resolution\": \"Cloud computing is often less expensive than on-premises datacenters because you are only billed for what you use. This pay-as-you-go model allows organizations to scale resources up or down based on demand, reducing unnecessary costs.\"\r\n    },\r\n" +
            "    {\r\n      \"Name\": \"Which cloud deployment model are you using if you have servers physically located at your organization’s on-site datacenter, and you migrate a few of the servers to the cloud?\",\r\n      \"Options\": [\r\n        {\r\n          \"Name\": \"hybrid cloud\",\r\n          \"IsCorrect\": true\r\n        },\r\n        {\r\n          \"Name\": \"private cloud\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"public cloud\",\r\n          \"IsCorrect\": false\r\n        }\r\n      ],\r\n      \"Hint\": \"Think about the combination of on-premises and cloud resources.\",\r\n      \"Resolution\": \"This scenario describes a hybrid cloud deployment model, where some resources remain on-premises while others are moved to the cloud.\"\r\n    },\r\n" +
            "    {\r\n      \"Name\": \"Select the answer that correctly completes the sentence.\",\r\n      \"Options\": [\r\n        {\r\n          \"Name\": \"disaster recovery\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"high availability\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"horizontal scaling\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"vertical scaling\",\r\n          \"IsCorrect\": true\r\n        }\r\n      ],\r\n      \"Hint\": \"Consider the process of increasing resources within the same server.\",\r\n      \"Resolution\": \"Increasing compute capacity by adding RAM or CPUs to a virtual machine is called vertical scaling, as it involves adding more resources to an existing machine.\"\r\n    },\r\n" +
            "    {\r\n      \"Name\": \"What is high availability in a public cloud environment dependent on?\",\r\n      \"Options\": [\r\n        {\r\n          \"Name\": \"capital expenditures\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"cloud-based backup retention limits\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"the service-level agreement (SLA) that you choose\",\r\n          \"IsCorrect\": true\r\n        },\r\n        {\r\n          \"Name\": \"the vertical scalability of an app\",\r\n          \"IsCorrect\": false\r\n        }\r\n      ],\r\n      \"Hint\": \"Think about the guarantees provided by the cloud provider for uptime and reliability.\",\r\n      \"Resolution\": \"High availability in a public cloud environment is primarily dependent on the service-level agreement (SLA) that you choose, which defines the level of uptime and reliability guaranteed by the provider.\"\r\n    }\r\n" +
            "  ]\r\n}\r\n" +
            "");


            var finalString = string.Join(" ", prompt);

            return finalString;
        }

        public static string GetNewQuestionFromPages(string bookArticle, IEnumerable<string> questionsAlreadyMade, int questionsAmount = 5, int incorrectOptionsAmount = 4)
        {
            if (string.IsNullOrEmpty(bookArticle))
            {
                throw new ArgumentException("Article content cannot be null or empty.", nameof(bookArticle));
            }

            StringBuilder promptBuilder = new StringBuilder();

            // Construindo o cabeçalho
            AddHeader(promptBuilder);


            // Adicionando o artigo
            promptBuilder.AppendLine("----------------------BEGIN OF ARTICLE-----------------------------------------------------------");
            promptBuilder.AppendLine(bookArticle);
            promptBuilder.AppendLine("----------------------END OF ARTICLE-----------------------------------------------------------");

            // Adicionando estrutura JSON
            promptBuilder.AppendLine("Provide questions and answers in the JSON structure below:");
            promptBuilder.AppendLine(GetJsonStructure(incorrectOptionsAmount));
            promptBuilder.AppendLine("Only use the information provided above. Do not add any content.");
            promptBuilder.AppendLine("Ensure that all answers (correct and incorrect) are similar in length and style to avoid making the correct answer stand out.");
            promptBuilder.AppendLine("The correct answer must not be longer, more detailed, or written in a noticeably different style compared to the incorrect answers.");
            promptBuilder.AppendLine("The incorrect answers should be plausible and equally detailed, so that the student must think critically to determine the right answer.");


            // Adicionando perguntas anteriores, se houver
            if (questionsAlreadyMade?.Any() == true)
            {
                AddPreviousQuestions(promptBuilder, questionsAlreadyMade);
            }

            // Configurando os detalhes das perguntas
            AddQuestionDetails(promptBuilder, questionsAmount, incorrectOptionsAmount);

            // Adicionando exemplo de pergunta
            AddExampleQuestion(promptBuilder);

            return promptBuilder.ToString();
        }

        private static void AddHeader(StringBuilder promptBuilder)
        {
            promptBuilder.AppendLine("As a specialist in constructing exams for the Azure AZ-900 Fundamentals Certification, " +
                                     "you need to create varied questions based strictly on the words and phrases in the provided text. " +
                                     "The questions should simulate real-world scenarios where the candidate needs to choose the best solution " +
                                     "based on practical use cases described in the text. Avoid using synonyms or creatively rephrasing the content. " +
                                     "Vary the structure of the questions, using different formats such as 'Which of the following', 'How can', 'Why would', 'When should', etc. " +
                                     "Ensure the product name appears in the answer choices when applicable, and avoid directly including it in the question." +
                                     "Focus on questions that challenge the candidate’s ability to apply knowledge practically and in detail. " +
                                     "Provide the questions and answers in the JSON format below:");
        }

        private static void AddPreviousQuestions(StringBuilder promptBuilder, IEnumerable<string> questionsAlreadyMade)
        {
            promptBuilder.AppendLine("You have already created similar questions from this article, don't repeat or create close variations of the same questions. Avoid similar questions, like:");

            foreach (var question in questionsAlreadyMade)
            {
                promptBuilder.AppendLine($"{question}");
            }

            promptBuilder.AppendLine("You must vary the questions you will make now, ensuring they are different in structure and context.");
        }

        private static void AddQuestionDetails(StringBuilder promptBuilder, int questionsAmount, int incorrectOptionsAmount)
        {
            promptBuilder.AppendLine($"Total questions: {questionsAmount}.");
            promptBuilder.AppendLine("Total Correct Answers: 1.");
            promptBuilder.AppendLine($"Total Incorrect Answers: {incorrectOptionsAmount}.");
            promptBuilder.AppendLine("Additional Note: \"Ensure that the questions and answers are strictly aligned with the provided text, without introducing new information or interpretations.\"");
        }

        private static void AddExampleQuestion(StringBuilder promptBuilder)
        {
            promptBuilder.AppendLine("Here is an example of a question with proper structure and variation: " +
                "{\r\n  \"Questions\": [\r\n    {\r\n      \"Name\": \"Which of the following is a cost-saving feature of cloud computing?\",\r\n      \"Options\": [\r\n        {\r\n          \"Name\": \"You only pay for what you use.\",\r\n          \"IsCorrect\": true\r\n        },\r\n        {\r\n          \"Name\": \"Cloud providers offer free hardware.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"Cloud services do not incur bandwidth costs.\",\r\n          \"IsCorrect\": false\r\n        },\r\n        {\r\n          \"Name\": \"Cloud services are only available in one region.\",\r\n          \"IsCorrect\": false\r\n        }\r\n      ],\r\n      \"Hint\": \"Consider how cloud providers bill for services and how they help organizations manage resources.\",\r\n      \"Resolution\": \"The correct answer is 'You only pay for what you use'. This reflects the pay-as-you-go model, which allows organizations to scale resources and reduce costs.\" \r\n    }\r\n  ]\r\n}");

            promptBuilder.AppendLine("\"Hint\" provides a clue, offering a subtle guide toward the correct answer without revealing it outright. It serves as a shortcut to help users think in the right direction.");
            promptBuilder.AppendLine("\"Resolution\" is a detailed and accurate explanation of the correct answer, providing full clarity and understanding.");
        }


        private static string GetJsonStructure(int incorrectOptionsAmount)
        {
            return $@"Format the JSON according to the provided description: 
                1. **`Questions`**: 
                   - Type: Array of objects.
                   - Description: A list of questions. Each object in the array represents a specific question.

                2. **Each object within the `Questions` array**:
                   - Type: Object.
                   - Description: Represents a specific question.

                   - **`Name`**:
                     - Type: String.
                     - Description: The text of the question. The question formulated about the provided text.

                   - **`Options`**:
                     - Type: Array of objects.
                     - Description: A list of answer options for the question. Each object in the array represents a possible answer.

                     - **Each object within the `Options` array**:
                       - Type: Object.
                       - Description: Represents a possible answer option.

                       - **`Name`**:
                         - Type: String.
                         - Description: The text of the answer option. The potential answer to the question.

                       - **`IsCorrect`**:
                         - Type: Boolean.
                         - Description: Indicates whether the option is the correct answer (`true`) or not (`false`). Only one option should be marked as correct (`true`).

                   - **`Hint`**:
                     - Type: String.
                     - Description: A hint to assist in answering the question. Should guide the respondent without directly revealing the correct answer.

                   - **`Resolution`**:
                     - Type: String.
                     - Description: A complete explanation of the correct answer. Should justify why the answer is correct and may mention the source of the question (e.g., the page of a document).

                **Note**: You should provide a total of {incorrectOptionsAmount} incorrect answer options for each question.";
        }
    }
}
