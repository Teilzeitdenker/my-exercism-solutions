using System;
using System.Collections.Generic;
using System.Linq;

public static class AffineCipher
{
    static List<int> Cands = new List<int> { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
    static bool IsNotValid(int a) => !Cands.Contains(a % 26);
    static int ModInverse26(int a) => Cands.Where(c => ((a % 26) * (c % 26)) % 26 == 1).First();
    static int LetterToInt(char c) => char.ToLower(c) - 'a';
    static char IntToLetter(int i)
    {
        if (i < 0) return IntToLetter(i + 26);
        else return (char)((i % 26) + 'a');
    }
    public static string Encode(string plainText, int a, int b)
    {
        if (IsNotValid(a)) throw new ArgumentException("a not valid");
        char AffineEncode(char c)
        {
            int x = LetterToInt(c);
            return IntToLetter(a * x + b);
        }
        int helper = 0;
         var listOf5s = plainText
                        .Where(char.IsLetterOrDigit)
                        .Select(c => char.IsDigit(c) ? c : AffineEncode(c))
                        .GroupBy(x => helper++ / 5).Select(s => new string(s.ToArray())).ToArray();
        return string.Join(' ', listOf5s);
    }

    public static string Decode(string cipheredText, int a, int b)
    {
        if (IsNotValid(a)) throw new ArgumentException("a not valid");
        char AffineDecode(char c)
        {
            int modInv = ModInverse26(a);
            int y = LetterToInt(c);
            return IntToLetter(modInv * (y - b));
        }
        return new(cipheredText
                    .Where(char.IsLetterOrDigit)
                    .Select(c => char.IsDigit(c) ? c : AffineDecode(c))
                    .ToArray()
                   );
    }
}
