using System;
using System.Linq;

public static class RotationalCipher
{
    public static string Rotate(string text, int shiftKey)
    {
        char[] chars = text.Select(c => char.IsLetter(c) ? shiftBy(c, shiftKey, char.IsUpper(c)) : c).ToArray();
        return new string(chars);
    }
    public static char shiftBy(char c, int shift, bool upper)
    {
        int zero = upper ? 'A' : 'a';
        return (char)(((c - zero) + shift) % 26 + zero);
    }
}