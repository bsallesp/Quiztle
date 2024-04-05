namespace BrunoTheBot.API.Prompts
{
    public static class LLMPrompts
    {
        public static string GetTopicsClassesToSchoolPrompt(string school, int topicClassAmount = 10)
        {
            return "{" +
                    "\"prompt\": \"What are the " + topicClassAmount + " most relevant classes of " + school + "?\"," +
                    "\"completion\": \"return array into a json answering: what is the " + topicClassAmount + " most relevant classes of " + school + "? {" +
                        "\\" + "\"" + school + "\\" + "\"" + ": [class1, class2, class3, so on]" +
                    "}\"" +
                "}";
        }

        public static string SearchSchoolPrompt(string schoolInput)
        {
            // Construir a prompt conforme o formato especificado
            string prompt = $"On a scale from zero to 10, what is the likelihood that '{schoolInput}' is a name of a college, course, or something that can be used to build a study plan?\n\n";
            prompt += "Return a JSON with this structure:\n\n";
            prompt += "{\n";
            prompt += "  \"probability\": From 0 to 10,\n";
            prompt += "  \"is_college\": true or false,\n";
            prompt += "  \"is_technical_course\": true or false,\n";
            prompt += "  \"is_general_knowledge_content\": true or false,\n";
            prompt += "  \"is_certification_program\": true or false,\n";
            prompt += "  \"is_workshop_or_seminar\": true or false,\n";
            prompt += "  \"is_online_course\": true or false,\n";
            prompt += "  \"is_something_not_fitting_college_course_or_study_plan\": true or false\n";
            prompt += "}\n";

            return prompt;
        }

        public static string SearchLearningContentPrompt(string input)
        {
            // Construir a prompt conforme o formato especificado
            string prompt = $"On a scale from 0 to 10, how likely is it that '{input}' can be used to create educational content related to a specific field or topic through artificial intelligence?\n\n";
            prompt += "Please provide a score from 0 to 10, where 0 indicates a low probability and 10 indicates a high probability.\n\n";
            prompt += "In your evaluation, consider whether the input represents a specific domain of knowledge, such as engineering, medicine, literature, or any other subject that can be used to create structured learning materials.";
            prompt += "Return a JSON with this structure:\n\n";
            prompt += "{\n";
            prompt += "  \"input\": [the input inserted],\n";
            prompt += "  \"probability\": From 0 to 10,\n";
            prompt += "}\n";

            return prompt;
        }

        public static string GetTopics(string input, int amount = 10)
        {
            // Construir a prompt conforme o formato especificado
            string prompt = $"Return a JSON containing {amount} subtopics related to the professional and technical topic '{input}'.\n\n";
            prompt += "{\n";
            prompt += $"  \"topic\": \"{input}\",\n";
            prompt += "  \"subtopics\": [topics such as algorithms, data structures, networking, cybersecurity, machine learning, artificial intelligence, software development methodologies, and more]\n";
            prompt += "}\n";

            return prompt;
        }

        //public static string GetTopicsAndReferencesOfSchool(bool literal = false, string school, int topicsAmount = 10, int referencesAmount = 3)
        //{
        //    //if (!literal) return $"return a json with main key = {school}, who has another json of {topicsAmount} keys," +
        //    //        $" and each keys have "

        //    string prompt = $"Return a JSON with this structure:";
        //    prompt += "{\n";
        //    prompt += $"  \"school\": \"{school}\",\n";
        //    prompt += $"  \"topics\": [divide the most important content about {school} in {topicsAmount} parts, in array format.]\n";
        //    prompt += "{\n";
        //    prompt += $"  \"references\": [each topic, must to have {referencesAmount} references about. ]\n";
        //    prompt += "}\n";
        //    prompt += "}\n";

        //    return prompt;
        //}

        public static string GetBestAuthors(string input, int amount = 10)
        {
            // Construir a prompt conforme o formato especificado
            string prompt = $"Return a JSON containing the top {amount} authors who are experts on the topic '{input}'.\n\n";
            prompt += "{\n";
            prompt += $"  \"topic\": \"{input}\",\n";
            prompt += "  \"authors\": [\n";
            for (int i = 1; i <= amount; i++)
            {
                prompt += $"    {{ \"name\": \"Author{i}\", \"score\": 0.0 }}";
                if (i < amount)
                {
                    prompt += ",";
                }
                prompt += "\n";
            }
            prompt += "  ]\n";
            prompt += "}\n";

            return prompt;
        }

        public static string BachelorDegrees(string input, int amount = 20)
        {
            // Construir a prompt conforme o formato especificado
            string prompt = $"Return a JSON containing the most relevant {amount} bachelor degrees worldwide, such as medicine, law, engineering, computer science, economics, psychology, biology, physics, mathematics, chemistry, architecture, business administration, environmental science, sociology, political science, literature, history, education, anthropology, and more.\n\n";

            return prompt;
        }
    }
}