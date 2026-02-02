using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

public static class ParallelLetterFrequency
{
    public static IDictionary<char, int> Calculate(IEnumerable<string> texts)
    {
        var cd = new ConcurrentDictionary<char, int>();
        Parallel.ForEach(texts, s =>
            Parallel.ForEach(s.ToLower().Where(c => Char.IsLetter(c)), c =>
                cd.AddOrUpdate(c, 1, (key, oldValue) => oldValue + 1)
            )
        );
        return cd;
    }
}