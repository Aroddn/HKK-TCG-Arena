using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HkkAllCardsToTCGArena
{
    public class CardFace
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public int? cost { get; set; } = 0;
        public string? image { get; set; }
        public bool? isHorizontal { get; set; } = false;
    }

    public class Card
    {
        public string? id { get; set; }
        public bool isToken { get; set; } = false;
        public Face? face { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }
        public int? cost { get; set; } = 0;
        [JsonPropertyName("Kiegészítő")]
        public string Kiegészítő { get; set; } = "";
    }


    public class Face
    {
        public CardFace? front { get; set; }
    }

    public static class JsonCreator
    {
        public static void CreateJson(int StartKey, string input)
        {
            if (string.IsNullOrEmpty(input)) { throw new ArgumentNullException("input"); }

            string pattern =
            @"kartya=(\d+)[^>]*>([^<]+)</a>\s*\(([^)]+)\)<br>";

            var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

            var sb = new StringBuilder();

            Console.WriteLine($"Found {matches.Count} matches.");

            foreach (Match match in matches)
            {
                string id = match.Groups[1].Value;
                string name = match.Groups[2].Value.Trim();
                string type = match.Groups[3].Value.Trim();

                sb.AppendLine($"{id} | {name} | {type}");
            }

            string[] lines = sb.ToString().Split(
            new[] { Environment.NewLine },
            StringSplitOptions.None);

            var cardsDict = new Dictionary<string, Card>();
            int startKey = StartKey;
            int currentKey = startKey;

            // Keyword mapping for type
            var typeMapping = new Dictionary<string, string>()
            {
                //Lények
                { "avatár", "Lény" },
                { "kalandozó", "Lény" },
                { "lény", "Lény" },// generic fallback
                { "szörny", "Lény" },
                { "galetki", "Lény" },
                { "torzszülött", "Lény" },

                //Sárga lap
                { "követő", "Követő" },
                { "szabálylap", "Szabálylap" },

                { "építmény", "Építmény" },
                { "tárgy", "Tárgy" },

                //Varázslatok
                { "általános", "Varázslat" },
                { "azonnali", "Varázslat" },
                { "bűbáj", "Varázslat" },

                //Egyéb
                { "szintlépés", "Szintlépés" },
                { "szakértelem", "Szakértelem" },
                { "céh", "Céh" },
                { "helyszín", "Helyszín" },
                { "őslich", "Őslich" },
                { "testrész", "Testrész" },

            };

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                if (parts.Length < 3) continue;

                string id = parts[0].Trim();
                string name = parts[1].Trim();
                string rawType = parts[2].Trim().ToLower();

                // Determine type based on keywords
                string type = "Egyéb"; // default
                foreach (var kv in typeMapping)
                {
                    if (rawType.Contains(kv.Key.ToLower()))
                    {
                        type = kv.Value;
                        break;
                    }
                }

                var card = new Card
                {
                    id = currentKey.ToString(),
                    name = name,
                    type = type,
                    cost = 0,
                    Kiegészítő = "Tiltott Városok",
                    face = new Face
                    {
                        front = new CardFace
                        {
                            name = name,
                            type = type,
                            cost = 0,
                            isHorizontal = false,
                            image = $"https://lapkereso.hkk.hu/HKKCardImage.php?cardID={id}&behoCK=982750446e405424602bbbb89c7a664eb7988474&showedKiadas=251"
                        }
                    }
                };

                // Add to dictionary with custom key
                cardsDict[currentKey.ToString()] = card;
                currentKey++;
            }

            // Serialize JSON
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(cardsDict, options);
            File.WriteAllText("cards3.json", json, Encoding.UTF8);

            Console.WriteLine($"Processed {cardsDict.Count} cards into cards4.json");
        }

    }
}
