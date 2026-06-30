using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

class Card
{
    public string id { get; set; }
    public bool isToken { get; set; } = false;
    public Face face { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public int cost { get; set; } = 0;
    public string Kiegészítő { get; set; }
}

class Face
{
    public Front front { get; set; }
}

class Front
{
    public string name { get; set; }
    public string type { get; set; }
    public int cost { get; set; } = 0;
    public string image { get; set; }
    public bool isHorizontal { get; set; } = false;
}

class Program
{
    static void Main(string[] args)
    {
        

        string outputFile = "cards.json";
        Dictionary<string, Card> cards = new Dictionary<string, Card>();

        // Load existing JSON if it exists
        if (File.Exists(outputFile))
        {
            string existingJson = File.ReadAllText(outputFile);
            if (!string.IsNullOrWhiteSpace(existingJson))
            {
                cards = JsonSerializer.Deserialize<Dictionary<string, Card>>(existingJson);
            }
        }

        

        Console.WriteLine("Enter cards in the format: Name URL (empty line to finish)");
        while (true)
        {
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) break;

            // Split input into name and URL
            int lastSpace = input.LastIndexOf(' ');
            if (lastSpace == -1)
            {
                Console.WriteLine("Invalid input format, try again.");
                continue;
            }

            string name = input.Substring(0, lastSpace).Trim();
            string url = input.Substring(lastSpace + 1).Trim();



            // Extract ID from URL
            Match match = Regex.Match(url, @"(\d{4,5})$");
            if (!match.Success)
            {
                Console.WriteLine("Could not find 4 or 5 digit ID at the end of URL.");
                continue;
            }
            string id = match.Groups[1].Value;


            // Determine type automatically: if name contains "2025" we could set something, else default
            
            //Lény
            //Varázslat
            //Tárgy
            //Építmény
            //Tárgy
            //Követő

            string type = "Céh"; // Default type, you can add logic for "Lény" etc. if needed
            int nextKey = cards.Count > 0 ? cards.Count + 11518 : 11518; // start from 43 or after existing count

            // Construct card object
            Card card = new Card
            {
                id = nextKey.ToString(),
                name = name,
                type = type,
                Kiegészítő = "Roxat céhei",
                face = new Face
                {
                    front = new Front
                    {
                        name = name,
                        type = type,
                        image = $"https://lapkereso.hkk.hu/HKKCardImage.php?cardID={id}&behoCK=982750446e405424602bbbb89c7a664eb7988474&showedKiadas=251"
                    }
                }
            };

            cards[nextKey.ToString()] = card;
            nextKey++;

            Console.WriteLine($"Added card: {name} with ID {id}");
        }

        // Save JSON to file
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(outputFile, JsonSerializer.Serialize(cards, options));

        Console.WriteLine($"JSON saved to {outputFile}");
    }
}
