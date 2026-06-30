namespace HkkAllCardsToTCGArena
{
    public class BontottMaker
    {
        public static void Run()
        {
            string gyakori = File.ReadAllText("Tiltott városok - gyakori.txt");
            string nemGyakori = File.ReadAllText("Tiltott városok - nem gyakori.txt");
            string ritka = File.ReadAllText("Tiltott városok - ritka.txt");

            List<string> allBontott = new();
            Random rng = new();

            for (int pack = 0; pack < 4; pack++)
            {
                allBontott.AddRange(OpenPack(gyakori, rng, 11));
                allBontott.AddRange(OpenPack(nemGyakori, rng, 5));
                allBontott.AddRange(OpenPack(ritka, rng, 2));
            }

            foreach (var group in allBontott
                .GroupBy(c => c)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key))
            {
                Console.WriteLine($"{group.Count()} {group.Key}");
            }

            var lines = allBontott
                .GroupBy(card => card)
                .OrderByDescending(g => g.Count())
                .Select(g => $"{g.Count()} {g.Key}");

            File.WriteAllLines("allBontott.txt", lines);
        }

        static List<string> OpenPack(string text, Random rng, int packSize = 11)
        {
            string[] gyakoriNevek = text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split('|')[1].Trim())
                .ToArray();

            return gyakoriNevek
                .OrderBy(_ => rng.Next())
                .Take(packSize)
                .ToList();
        }
    }
}