using System;
using System.Collections.Generic;
using System.Linq;

public static class Dominoes
{
    public static bool CanChain(IEnumerable<(int, int)> ds)
    {
        if (!ds.Any()) // empty enum
            return true;
        var (a, b) = ds.ElementAt(0); // first element
        if (!ds.Skip(1).Any()) // exactly one domino
            return a == b;
        for (int i = 1; i < ds.Count(); i++) // go through all other dominoes
        {
            var cand = ds.ElementAt(i);
            if (Matches(b, cand)) // check if it chains
            {
                var rstDs = ds.Where((dominoe, idx) => idx != i).Skip(1); // throw away the candidate and the front domino
                if (CanChain(rstDs.Prepend(GetMatch(a, b, cand)))) return true; // then prepend the (a, unmatching) domino and check if one canChain this
            }
        }
        return false; 
    }
    // checks if a candidate matches the back number of the first domino
    private static bool Matches(int bck, (int, int) cand) => 
        bck == cand.Item2 || bck == cand.Item1;
    // this returns a domino made of the old front number and the unmatching number from the candidate
    private static (int, int) GetMatch(int frt, int bck, (int, int) cand) {
        if (bck == cand.Item1) return (frt, cand.Item2); 
        return (frt, cand.Item1); 
    }
}