using System;
using System.Text.Json;

namespace Quiztle.CoreBusiness.Utils
{
    public static class JsonExtractor
    {
        public static string ExtractFromLLMResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                throw new ArgumentException("Response cannot be null or empty", nameof(response));
            }

            // Encontrar o início e o fim do JSON na resposta
            int jsonStartIndex = response.IndexOf("```json", StringComparison.Ordinal);
            int jsonEndIndex = response.IndexOf("```", jsonStartIndex + 1, StringComparison.Ordinal);

            if (jsonStartIndex == -1 || jsonEndIndex == -1)
            {
                throw new InvalidOperationException("JSON not found in response");
            }

            // Ajustar os índices para pegar o JSON
            jsonStartIndex += "```json".Length; // Pular a marcação do início
            string jsonPart = response.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex).Trim();

            // Validar se é um JSON válido
            try
            {
                JsonDocument.Parse(jsonPart);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Extracted text is not a valid JSON", ex);
            }

            return jsonPart;
        }

        public static bool TryFindGuidInJson(string json, out Guid foundGuid)
        {
            foundGuid = Guid.Empty; // Valor padrão
            try
            {
                using JsonDocument document = JsonDocument.Parse(json);
                return SearchForGuid(document.RootElement, out foundGuid);
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
                return false; // Indica que a busca não foi bem-sucedida
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false; // Indica que a busca não foi bem-sucedida
            }
        }

        private static bool SearchForGuid(JsonElement element, out Guid foundGuid)
        {
            // Inicializa o GUID encontrado
            foundGuid = Guid.Empty;

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                    {
                        // Verifica a chave da propriedade como uma string
                        if (Guid.TryParse(property.Name, out foundGuid) ||
                            SearchForGuid(property.Value, out foundGuid))
                        {
                            return true; // GUID encontrado
                        }
                    }
                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                    {
                        if (SearchForGuid(item, out foundGuid))
                        {
                            return true; // GUID encontrado
                        }
                    }
                    break;

                case JsonValueKind.String:
                    if (Guid.TryParse(element.GetString(), out foundGuid))
                    {
                        return true; // GUID encontrado
                    }
                    break;

                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                    break; // Não faz nada para esses tipos

                default:
                    break;
            }

            return false; // GUID não encontrado
        }
    }
}
