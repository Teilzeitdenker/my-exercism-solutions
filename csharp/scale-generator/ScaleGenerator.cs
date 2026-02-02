using System;
using System.Collections.Generic;
using System.Linq;

public static class ScaleGenerator
{
    private static readonly List<string> sharps = new List<string> { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
    private static readonly List<string> flats = new List<string> { "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab" };
    private static readonly List<string> sharpTonics = new List<string> { "C", "G", "D", "A", "E", "B", "F#", "a", "e", "b", "f#", "c#", "g#", "d#" };
    private static readonly List<string> flatTonics = new List<string> { "F", "Bb", "Eb", "Ab", "Db", "Gb", "d", "g", "c", "f", "bb", "eb" };
    public static string[] Chromatic(string tonic)
    {
        if (sharpTonics.Contains(tonic))
        {
            tonic = GetInvariantTonic(tonic);
            return sharps
                .SkipWhile(tone => tone != tonic)
                .Take(12)
                .ToArray();
        }
        else if (flatTonics.Contains(tonic))
        {
            tonic = GetInvariantTonic(tonic);
            return flats
                .SkipWhile(tone => tone != tonic)
                .Take(12)
                .ToArray();
        }
        else throw new ArgumentException("No such tonic!");

    }

    public static string[] Interval(string tonic, string pattern)
    {
        bool[] filterPattern = pattern.SelectMany(c => c switch
       {
           'm' => new[] { true },
           'M' => new[] { true, false },
           'A' => new[] { true, false, false },
           _   => throw new ArgumentException("No such interval")
       }).Append(false).Append(false).ToArray();
        return Chromatic(tonic)
                .Where((tone, i) => filterPattern[i])
                .ToArray();
    }

    public static string GetInvariantTonic(string tonic)
    {
        if (tonic.Length == 1) return tonic.ToUpper();
        else if (tonic.Length == 2) return tonic[0].ToString().ToUpper() + tonic[1].ToString();
        else throw new ArgumentException("No such tonic!");
    }

}