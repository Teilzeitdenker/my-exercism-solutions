using System;
using System.Linq;

public static class PigLatin
{
    public static string Translate(string word)
    {
        // recurse on phrases
        if (word.Contains(' '))
        {
            return String.Join(' ', word.Split(' ').Select(w => Translate(w)));
        }
        var vowels = new char[]{ 'a', 'e', 'i', 'o', 'u' };
        var fst_vowel = word.IndexOfAny(vowels);
        // Rule 1
        if (fst_vowel == 0 || word.StartsWith("xr") || word.StartsWith("yt"))
        {
            return word + "ay";
        }
        // Rule 4
        if (word.Length == 2 && word[1] == 'y')
        {
            return "y" + word[0] + "ay";
        }
        var fst_yt = word.IndexOf("yt");
        if (fst_yt != -1)
        {
            return word.Substring(fst_yt) + word.Substring(0, fst_yt) + "ay";
        }
        // Rule 3
        var consclust = word.Substring(0, fst_vowel);
        var fst_qu = word.IndexOf("qu");
        if (fst_qu == fst_vowel - 1)
        {
            return word.Substring(fst_vowel + 1) + consclust + "uay";
        }    
        // Rule 2
        return word.Substring(fst_vowel) + consclust + "ay";
    }
}