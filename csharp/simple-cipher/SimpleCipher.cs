using System;
using System.Linq;

public class SimpleCipher
{
    private string _key;
    public SimpleCipher()
    {
        Random rand = new Random();
        int zero = 'a';
        _key = new string(Enumerable.Range(0, 100).Select(_ => (char)(rand.Next(26) + zero)).ToArray());
    }
    public SimpleCipher(string key)
    {
        _key = key;
    }
    public string Key
    {
        get
        {
            return _key;
        }
    }
    public string Encode(string text)
    {
        int repetitions = text.Length / Key.Length + 1;
        string longerKey = String.Concat(Enumerable.Repeat(Key, repetitions));
        char[] chars = text.Zip(longerKey, (c, k) => char.IsLetter(c) ? ShiftEncode(c, (int)k - (int)'a') : c).ToArray();
        return new string(chars);
    }
    public string Decode(string text)
    {
        int repetitions = text.Length / Key.Length + 1;
        string longerKey = String.Concat(Enumerable.Repeat(Key, repetitions));
        char[] chars = text.Zip(longerKey, (c, k) => char.IsLetter(c) ? ShiftDecode(c, (int)k - (int)'a') : c).ToArray();
        return new string(chars);
    }
    public static char ShiftEncode(char c, int shift)
    {
        int zero = 'a';
        return (char)(((c - zero) + shift + 26) % 26 + zero);
    }
    public static char ShiftDecode(char c, int shift)
    {
        int zero = 'a';
        return (char)(((c - zero) - shift + 26) % 26 + zero);
    }
}