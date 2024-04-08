using BrunoTheBot.CoreBusiness.Entities.Course;
using Newtonsoft.Json.Linq;

namespace BrunoTheBot.API.Services
{
    public static class JSONConverter
    {
        public static List<TopicClass> ConvertToTopicClasses(string input, string key)
        {
            try
            {
                var content = ExtractChatGPTResponseFromJSON(input);
                JObject contentData = JObject.Parse(content);
                var topicClassesArray = (JArray)contentData[key]! ?? throw new Exception("TopicClasses not found in JSON.");

                List<TopicClass> _topicClassesList = new(topicClassesArray.Count);

                foreach (var item in topicClassesArray)
                {
                    var topicClass = new TopicClass
                    {
                        Name = item.ToString()
                    };
                    _topicClassesList.Add(topicClass);

                    Console.WriteLine("Adding " + item.ToString());
                }

                if (_topicClassesList.Count == 0 || _topicClassesList == null) throw new Exception();

                return _topicClassesList;
            }

            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
        }

        public static List<Section> ConvertToSections(string input, string key)
        {
            try
            {
                var sectionResponse = ExtractChatGPTResponseFromJSON(input);
                JObject sectionJSONObject = JObject.Parse(sectionResponse);

                var sectionsArray = (JArray)sectionJSONObject[key]! ?? throw new Exception("TopicClasses not found in JSON.");

                List<Section> sectionsList = new List<Section>(sectionsArray.Count);

                foreach (var section in sectionsArray)
                {
                    sectionsList.Add(new Section
                    {
                        Name = (string)section!
                    }) ;
                }


                if (sectionsList.Count == 0 || sectionsList == null) throw new Exception();

                return sectionsList;
            }


            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
        }

        public static string ConvertToContent(string input, string key)
        {
            try
            {
                var content = ExtractChatGPTResponseFromJSON(input);
                JObject contentData = JObject.Parse(content);
                string newContent = contentData[key]?.ToString() ?? throw new Exception("Got an error: ");
                return newContent;
            }


            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
        }

        public static string ExtractChatGPTResponseFromJSON(string input)
        {
            try
            {
                JObject jsonObject = JObject.Parse(input);

#pragma warning disable CS8600
                string content = (string)jsonObject["choices"]![0]!["message"]!["content"] ?? throw new Exception();
#pragma warning restore CS8600

                return content;
            }

            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção: {ex.Message}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $" StackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
        }
    }
}