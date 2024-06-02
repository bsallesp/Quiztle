namespace BrunoTheBot.Services
{
    public static class RandomNames
    {
        private static readonly Random _random = new Random();
        private static readonly string[] Words = new[]
        {
            "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta", "Iota", "Kappa",
            "Lambda", "Mu", "Nu", "Xi", "Omicron", "Pi", "Rho", "Sigma", "Tau", "Upsilon",
            "Phi", "Chi", "Psi", "Omega", "Aqua", "Terra", "Ignis", "Aer", "Lux",
            "Tenebris", "Vita", "Mortem", "Fortis", "Bellum", "Pax", "Sol", "Luna", "Stella", "Nox",
            "Aurora", "Aether", "Chaos", "Cosmos", "Nexus", "Quantum", "Nova", "Nebula", "Vortex", "Zenith",
            "Abyss", "Blaze", "Celestial", "Draco", "Ember", "Frost", "Gladius", "Horizon", "Inferno", "Jade",
            "Krypton", "Leviathan", "Mystic", "Nimbus", "Obsidian", "Phoenix", "Quasar", "Ragnarok", "Sapphire", "Titan",
            "Umbra", "Viper", "Wyvern", "Xenon", "Ymir", "Zephyr", "Aegis", "Blitz", "Chimera", "Dusk",
            "Eclipse", "Falcon", "Glimmer", "Haven", "Ion", "Juggernaut", "Kraken", "Lyra", "Mirage", "Nyx",
            "Onyx", "Paragon", "Quintessence", "Radiance", "Scion", "Tempest", "Utopia", "Vanguard", "Wraith", "Xylon",
            "Yggdrasil", "Zen", "Aether", "Basilisk", "Cascade", "Dynamo", "Echo", "Fury", "Gryphon", "Hydra",
            "Illusion", "Jolt", "Karma", "Lynx", "Miracle", "Nebula", "Oracle", "Pulse", "Quake", "Rift",
            "Serenity", "Torrent", "Unity", "Valor", "Whisper", "Xenith", "Yield", "Zenobia", "Abyssal", "Borealis",
            "Crescent", "Deluge", "Eon", "Fable", "Gale", "Halcyon", "Inspire", "Jubilee", "Kismet", "Luminous",
            "Mystery", "Noble", "Odyssey", "Pioneer", "Quest", "Reverie", "Starlight", "Tundra", "Vista", "Wander",
            "Xylo", "Yearn", "Zealot"
        };

        public static List<string> GenerateRandomNames(int count)
        {
            var randomNames = new HashSet<string>();

            while (randomNames.Count < count)
            {
                var name = $"{GetRandomWord()} {GetRandomWord()} {GetRandomWord()}";
                randomNames.Add(name);
            }

            return randomNames.ToList();
        }

        private static string GetRandomWord()
        {
            return Words[_random.Next(Words.Length)];
        }

        public static string GenerateConcatenatedNames(int count)
        {
            var randomNames = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var name = $"{GetRandomWord()} {GetRandomWord()} {GetRandomWord()}";
                randomNames.Add(name);
            }

            return string.Join(", ", randomNames);
        }
    }
}
