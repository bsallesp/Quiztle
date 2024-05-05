using BrunoTheBot.CoreBusiness.Entities.Course;
using BrunoTheBot.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace BrunoTheBot.API.Services
{
    public static class JSONConverter
    {
        public static List<Chapter> ConvertToChapters(string input, string key)
        {
            try
            {
                var content = ExtractChatGPTResponseFromJSON(input);
                JObject contentData = JObject.Parse(content);
                var chaptersArray = (JArray)contentData[key]! ?? throw new Exception("Chapters not found in JSON.");

                List<Chapter> _chaptersList = new(chaptersArray.Count);

                foreach (var item in chaptersArray)
                {
                    var chapter = new Chapter
                    {
                        Name = item.ToString()
                    };
                    _chaptersList.Add(chapter);

                    Console.WriteLine("Adding " + item.ToString());
                }

                if (_chaptersList.Count == 0 || _chaptersList == null) throw new Exception();

                return _chaptersList;
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
            var sectionResponse = ExtractChatGPTResponseFromJSON(input);
            JObject sectionJSONObject = JObject.Parse(sectionResponse);

            try
            {
                var sectionsArray = (JArray)sectionJSONObject[key]! ?? throw new Exception("Chapters not found in JSON.");

                List<Section> sectionsList = new List<Section>(sectionsArray.Count);

                foreach (var section in sectionsArray)
                {
                    sectionsList.Add(new Section
                    {
                        Name = (string)section!
                    });
                }


                if (sectionsList.Count == 0 || sectionsList == null) throw new Exception();

                return sectionsList;
            }


            catch (Exception ex)
            {
                string errorMessage = $"Failed during converting: {ex.Message}\n\n" +

                "Content that failed to convert:\n" +
                $"{input}\n\n" +

                "JSON that failed to convert:\n" +
                $"{sectionJSONObject.ToString()}\n\n";

                if (ex.InnerException != null)
                {
                    errorMessage += $" InnerException: {ex.InnerException.Message}";
                }

                errorMessage += $" StackTrace: {ex.StackTrace}";

                throw new Exception(errorMessage);
            }
        }

        public static Question ConvertToQuestion(string input)
        {
            try
            {
                var questionResponse = ExtractChatGPTResponseFromJSON(input);
                JObject sectionJSONObject = JObject.Parse(questionResponse);

                Question newQuestion = new Question
                {
                    Name = (string)sectionJSONObject["Question"]! ?? throw new Exception("Question not found in JSON."),
                    Answer = (string)sectionJSONObject["Answer"]! ?? throw new Exception("Answer not found in JSON."),
                    Options = new List<Option>
                    {
                        new Option
                        {
                            Name = (string)sectionJSONObject["Option1"]! ?? throw new Exception("Option1 not found in JSON.")
                        },
                        new Option
                        {
                            Name = (string)sectionJSONObject["Option2"]! ?? throw new Exception("Option2 not found in JSON.")
                        },
                        new Option
                        {
                            Name = (string)sectionJSONObject["Option3"]! ?? throw new Exception("Option3 not found in JSON.")
                        },
                        new Option
                        {
                            Name = (string)sectionJSONObject["Option4"]! ?? throw new Exception("Option4 not found in JSON.")
                        }
                    },

                    Hint = (string)sectionJSONObject["Hint"]! ?? throw new Exception("Hint not found in JSON.")
                };

                return newQuestion;
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
                // Verifica se o JSON de entrada está vazio
                if (string.IsNullOrEmpty(input))
                {
                    throw new ArgumentException("O JSON de entrada está vazio ou nulo.");
                }

                // Analisa o JSON de entrada
                JObject jsonObject = JObject.Parse(input);

                // Verifica se o JSON contém a chave "choices" e se tem pelo menos um item no array
                if (jsonObject["choices"] == null || !jsonObject["choices"].Any())
                {
                    throw new ArgumentException("O JSON de entrada não contém a chave 'choices' ou está vazio.");
                }

                // Obtém o conteúdo da primeira mensagem no primeiro item do array "choices"
                string content = (string)jsonObject["choices"][0]?["message"]?["content"];

                // Verifica se o conteúdo é nulo ou vazio
                if (string.IsNullOrEmpty(content))
                {
                    throw new ArgumentException("O conteúdo da mensagem no JSON de entrada está vazio ou nulo.");
                }

                return content;
            }
            catch (Exception ex)
            {
                // Captura e retorna informações detalhadas da exceção
                string errorMessage = $"Ocorreu uma exceção ao extrair resposta do ChatGPT JSON: {ex.Message}";

                // Adiciona o JSON de entrada à mensagem de erro
                errorMessage += $"\nJSON de entrada: {input}";

                // Verifica se a exceção possui uma causa (InnerException)
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInnerException: {ex.InnerException.Message}";
                }

                // Adiciona outras propriedades da exceção, se necessário
                errorMessage += $"\nStackTrace: {ex.StackTrace}";

                // Lança uma nova exceção com a mensagem detalhada
                throw new Exception(errorMessage);
            }
        }
    }
}