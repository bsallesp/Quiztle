namespace Quiztle.API.Services
{
    public class OpenAITokenManager
    {
        public static List<string> SplitTextIntoTokenSafeParts(string input, int maxWordsPerPart = 3000)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            if (maxWordsPerPart <= 0)
                throw new ArgumentException("Maximum words per part must be greater than zero.", nameof(maxWordsPerPart));

            var words = input.Split(new[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();

            int wordCount = 0;
            var currentPart = new List<string>();

            foreach (var word in words)
            {
                if (wordCount + 1 > maxWordsPerPart)
                {
                    result.Add(string.Join(" ", currentPart));
                    currentPart.Clear();
                    wordCount = 0;
                }

                currentPart.Add(word);
                wordCount++;
            }

            if (currentPart.Count > 0)
            {
                result.Add(string.Join(" ", currentPart));
            }

            return result;
        }
    }
}
