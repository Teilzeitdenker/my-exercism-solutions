using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;

public static class WordCount
{
    public static IDictionary<string, int> CountWords(string phrase)
    {
        var cd = new ConcurrentDictionary<string, int>();
        char[] seps = new[] { '\t', '\n', ' ', ',', '.' };
        var chunks = phrase.Split(seps, StringSplitOptions.RemoveEmptyEntries);
        string NormalizeWord(string s)
        {
            int len = s.Length;
            return new(s
                        .ToLower()
                        .Where( (c, ind) => 
                            char.IsLetterOrDigit(c) || 
                            (c == '\'' && ind != 0 && ind != len - 1))
                        .ToArray());
        }
        Parallel.ForEach(chunks, s => cd.AddOrUpdate(NormalizeWord(s), 1, (key, oldValue) => oldValue + 1));
        return cd;        
    }                       
}   
