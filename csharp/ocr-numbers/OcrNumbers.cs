using System;
using System.Linq;
using System.Collections.Generic;

public static class OcrNumbers
{
    public static string Convert(string input)
    {
        var rows = input.Split('\n');
        if (rows.Length % 4 != 0 || rows[0].Length % 3 != 0) throw new ArgumentException();
        return string.Join(",", rows.Chunk(4).Select(row => getAllDigits(row)));
    }
    private static string getAllDigits(string[] row)
    {
        int numDigits = row[0].Length / 3;
        var ocrDigits = Enumerable.Repeat(String.Empty, numDigits).ToArray();
        foreach (string line in row)
        {   // in F# there's this nice way of transposing lists by just calling List.transpose
            string[] parts = ChunkStringIn3s(line);
            for (int i = 0; i < numDigits; i++)
            {
                ocrDigits[i] += parts[i];
            }
        }
        return string.Join("", ocrDigits.Select(d => Decode(d)));
    }

    private static string[] ChunkStringIn3s(string s)
    {
        return Enumerable.Range(0, s.Length / 3).Select(i => s.Substring(i * 3, 3)).ToArray();
    }

    private static string Decode(string s)
    {
        foreach (string key in DecodeMap.Keys)
        {
            if (DecodeMap[key] == s) return key;
        }
        return "?";
    }

    private static Dictionary<string, string> DecodeMap = new Dictionary<string, string>()
    {
        {"0", " _ " +
              "| |" +
              "|_|" +
              "   " },
        {"1", "   " +
              "  |" +
              "  |" +
              "   " },
        {"2", " _ " +
              " _|" +
              "|_ " +
              "   " },
        {"3", " _ " +
              " _|" +
              " _|" +
              "   " },
        {"4", "   " +
              "|_|" +
              "  |" +
              "   " },
        {"5", " _ " +
              "|_ " +
              " _|" +
              "   " },
        {"6", " _ " +
              "|_ " +
              "|_|" +
              "   " },
        {"7", " _ " +
              "  |" +
              "  |" +
              "   " },
        {"8", " _ " +
              "|_|" +
              "|_|" +
              "   " },
        {"9", " _ " +
              "|_|" +
              " _|" +
              "   " }
    };
}