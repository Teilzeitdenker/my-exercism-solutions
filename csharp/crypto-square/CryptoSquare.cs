using System;
using System.Collections.Generic;
using System.Linq;

public static class CryptoSquare
{
    public static string NormalizedPlaintext(string plaintext)
    {
        return new String(plaintext
                .Where(c => char.IsLetter(c) || char.IsDigit(c)).Select(c => char.ToLower(c)).ToArray());
    }

    public static IEnumerable<string> PlaintextSegments(string plaintext)
    {
        int length = plaintext.Length;
        int c = (int)Math.Ceiling(Math.Sqrt(length));
        for (int i = 0; i < length; i += c)
        {
            yield return plaintext.Substring(i, Math.Min(c, length - i));
        }

    }

    public static IEnumerable<string> Encoded(string plaintext)
    {
        string[] segs = PlaintextSegments(NormalizedPlaintext(plaintext)).ToArray();
        char safe_index(string segment, int i)
        {
            if (i >= segment.Length) return ' ';
            return segment[i];
        }
        return Enumerable.Range(0, segs[0].Length)
                .Select(n => 
                    new String(
                        segs
                        .Select(seg => safe_index(seg, n))
                        .ToArray()
                        )
                    );
    }

    public static string Ciphertext(string plaintext)
    {
        if (plaintext.Length == 0) return "";
        return String.Join(' ', Encoded(plaintext));
    }
}