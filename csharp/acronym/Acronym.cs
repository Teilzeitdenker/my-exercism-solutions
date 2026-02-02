using System;
using System.Linq;

public static class Acronym
{
    public static string Abbreviate(string phrase)
    {
        char[] delimiters = new[] { ' ', '-', '_' };
        return phrase.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Select(word => Char.ToUpper(word[0]).ToString()).Aggregate( (w, v) => w + v );
    }
}