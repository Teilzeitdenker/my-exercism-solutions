using System;
using System.Collections.Generic;

public static class Pangram
{
    public static bool IsPangram(string input)
    {
        HashSet<string> alphabet = new HashSet<string>();
        foreach (char c in input)
            if (char.IsLetter(c))
                alphabet.Add(c.ToString().ToLower());
        if (alphabet.Count == 26) return true;
        else return false;
    }
}
