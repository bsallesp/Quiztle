using Humanizer;

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

            Console.WriteLine(finalString);

            return finalString;
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
