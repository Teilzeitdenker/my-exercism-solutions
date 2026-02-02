using System;
using System.Collections.Generic;
using System.Linq;

public static class AtbashCipher
{
    static IEnumerable<char> a_To_z = Enumerable.Range('a', 26).Select(c => (char)c);
    static Dictionary<char, char> CipherMap = a_To_z.Zip(a_To_z.Reverse()).ToDictionary(x => x.First, x => x.Second);
    static char Atbash(char c) => char.IsDigit(c) ?  c : CipherMap[c]; 
    static IEnumerable<string> GetEncodedChunks(string plainValue) =>
        plainValue
            .Where(char.IsLetterOrDigit)
            .Select(char.ToLower)
            .Select(Atbash)
            .Chunk(5)
            .Select(chunk => new string(chunk));
    public static string Encode(string plainValue) => string.Join(" ", GetEncodedChunks(plainValue));
    public static string Decode(string encodedValue) => string.Join("", GetEncodedChunks(encodedValue));
}
