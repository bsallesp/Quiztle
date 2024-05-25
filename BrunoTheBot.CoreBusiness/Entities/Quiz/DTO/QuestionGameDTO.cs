namespace BrunoTheBot.CoreBusiness.Entities.Quiz.DTO
{
    public class QuestionGameDTO
    {
        public Guid Id { get; set; }
        public string Question { get; set; } = "";
        public Dictionary<string, (bool, string)> Options { get; set; } = [];

        // Método para embaralhar as opções
        public void ShuffleOptions()
        {
            Random rng = new Random();
            var pairs = Options.ToList();
            int n = pairs.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var pair = pairs[k];
                pairs[k] = pairs[n];
                pairs[n] = pair;
            }
            Options.Clear();
            foreach (var pair in pairs)
            {
                Options.Add(pair.Key, pair.Value);
            }
        }

        public bool IsSelectedOptionCorrect(string selectedOptionKey)
        {
            if (Options.TryGetValue(selectedOptionKey, out var option))
            {
                return option.Item1;
            }
            else
            {
                throw new ArgumentException("Chave de opção inválida.");
            }
        }

        public void SetAllOptionsFalse()
        {
            foreach (var key in Options.Keys.ToList())
            {
                Options[key] = (false, Options[key].Item2);
            }
        }
        public void SetOptionTrue(string optionKey)
        {
            if (Options.TryGetValue(optionKey, out var option))
            {
                Options[optionKey] = (true, option.Item2);
            }
            else
            {
                throw new ArgumentException("Chave de opção inválida.");
            }
        }
    }
}
