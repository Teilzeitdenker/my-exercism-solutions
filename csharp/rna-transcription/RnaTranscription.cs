using System;
using System.Text;
using System.Collections.Generic;

public static class RnaTranscription
{
    private static Dictionary<char, char> _transcriptions = new Dictionary<char, char>()
    {
        ['G'] = 'C',
        ['C'] = 'G',
        ['T'] = 'A',
        ['A'] = 'U'
    };
    public static string ToRna(string nucleotide)
    {
        StringBuilder rna = new StringBuilder(nucleotide.Length);
        foreach (char c in nucleotide)
        {
            if (!_transcriptions.ContainsKey(c)) throw new ArgumentException("Invalid nucleotide!");
            else rna.Append(_transcriptions[c]);
        }
        return rna.ToString();
    }
}