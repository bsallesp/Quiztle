using System;
using System.Collections.Generic;

namespace BrunoTheBot.Blazor.Client.APIServices
{
    public class CodeExtraction
    {
        public static Dictionary<string, string> ExtractCode(string input)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Inicializa o texto sem código como a entrada original
            string textWithoutCode = input;

            // Inicializa o código extraído como uma string vazia
            string extractedCode = "";

            // Encontra todas as ocorrências de #BEGINCODE# e #ENDCODE# na string
            int beginIndex = input.IndexOf("#BEGINCODE#");
            while (beginIndex != -1)
            {
                // Encontra o índice do marcador #ENDCODE# correspondente
                int endIndex = input.IndexOf("#ENDCODE#", beginIndex);

                // Se o marcador #ENDCODE# não for encontrado, encerra o loop
                if (endIndex == -1)
                    break;

                // Extrai o código entre os marcadores
                string code = input.Substring(beginIndex + "#BEGINCODE#".Length, endIndex - (beginIndex + "#BEGINCODE#".Length));

                // Remove o código da string original
                textWithoutCode = textWithoutCode.Remove(beginIndex, endIndex + "#ENDCODE#".Length - beginIndex);

                // Adiciona o código extraído ao resultado
                extractedCode += code + Environment.NewLine;

                // Procura pela próxima ocorrência de #BEGINCODE#
                beginIndex = input.IndexOf("#BEGINCODE#", endIndex);
            }

            // Adiciona o texto sem código e o código extraído ao resultado
            result["text"] = textWithoutCode;
            result["code"] = extractedCode.TrimEnd(); // Remove espaços em branco no final do código

            return result;
        }
    }
}
