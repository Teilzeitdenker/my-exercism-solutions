using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public static class FoodChain
{
    private static readonly string[] _tiere = { "", "fly", "spider", "bird", "cat", "dog", "goat", "cow", "horse" };
    private static readonly Dictionary<int, string> _ausrufe = new()
    {
        {1, "I don't know why she swallowed the fly. Perhaps she'll die." },
        {2, "It wriggled and jiggled and tickled inside her." },
        {3, "How absurd to swallow a bird!" },
        {4, "Imagine that, to swallow a cat!" },
        {5, "What a hog, to swallow a dog!" },
        {6, "Just opened her throat and swallowed a goat!" },
        {7, "I don't know how she swallowed a cow!" },
        {8, "She's dead, of course!" }
    };

    public static string Recite(int verseNumber)
    {
        string ersterTeil = $"I know an old lady who swallowed a {_tiere[verseNumber]}.\n{_ausrufe[verseNumber]}";
        if (_tiere[verseNumber] == "horse" || verseNumber == 1) return ersterTeil;
        StringBuilder ganzerText = new StringBuilder();
        ganzerText.Append(ersterTeil);
        for (int i = verseNumber; i > 1 ; i--)
        {
            ganzerText.Append($"\nShe swallowed the {_tiere[i]} to catch the {_tiere[i - 1]}{(_tiere[i - 1] == "spider" ? " that wriggled and jiggled and tickled inside her" : "")}.");
        }
        ganzerText.Append("\n" + _ausrufe[1]);
        return ganzerText.ToString();
    }

    public static string Recite(int startVerse, int endVerse) => string.Join("\n\n", Enumerable.Range(startVerse, endVerse - startVerse + 1).Select(x => Recite(x)));
}