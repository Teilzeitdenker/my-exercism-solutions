using System;
using System.Collections.Generic;

public static class Etl
{
    public static Dictionary<string, int> Transform(Dictionary<int, string[]> old)
    {
        Dictionary<string, int> better = new Dictionary<string, int>();
        foreach (int i in old.Keys)
        {
            foreach (string letter in old[i])
            {
                better[letter.ToLower()] = i;
            }
        }
        return better;
    }
}