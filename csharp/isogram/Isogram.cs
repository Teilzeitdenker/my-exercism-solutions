using System;
using System.Collections.Generic;

public static class Isogram
{
    public static bool IsIsogram(string word)
    {
        HashSet<char> appeared = new HashSet<char>();
        foreach (char c in word)
        {
            if (char.IsLetter(c))
            {
                if (appeared.Contains(char.ToLower(c))) return false;
                appeared.Add(char.ToLower(c));
            }        
        }
        return true;
    }
}
