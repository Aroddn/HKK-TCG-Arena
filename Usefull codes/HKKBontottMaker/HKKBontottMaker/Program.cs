using System;
using System.Collections.Generic;
using System.Linq;

string gyakori = File.ReadAllText("Tiltott városok - gyakori.txt");
string nemGyakori = File.ReadAllText("Tiltott városok - nem gyakori.txt");
string ritka = File.ReadAllText("Tiltott városok - ritka.txt");
List<string> allBontott = new();

Random rng = new();
static List<string> OpenPack(string gyakoriText, Random rng, int packSize = 11)
{
    string[] gyakoriNevek = gyakoriText
        .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
        .Select(line => line.Split('|')[1].Trim())
        .ToArray();

    List<string> shuffledGyakori = gyakoriNevek
        .OrderBy(_ => rng.Next())
        .ToList();

    return shuffledGyakori.Take(packSize).ToList();
}

for (int pack = 0; pack < 4; pack++)
{
    var packCardsGyakori = OpenPack(gyakori, rng, 11);
    allBontott.AddRange(packCardsGyakori);

    var packCardsNemGyakori = OpenPack(nemGyakori, rng, 5);
    allBontott.AddRange(packCardsNemGyakori);

    var packCardsRitka = OpenPack(ritka, rng, 2);
    allBontott.AddRange(packCardsRitka);
}

// Display totals
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