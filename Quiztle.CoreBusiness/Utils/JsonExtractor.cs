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
    }
}
