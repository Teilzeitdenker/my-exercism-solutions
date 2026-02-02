using System;
using System.Linq;
using System.Collections.Generic;

public static class Poker
{
    private static (T1[], T2[]) Unzip<T1, T2>(this IEnumerable<(T1, T2)> src) => (src.Select(t => t.Item1).ToArray(), src.Select(t => t.Item2).ToArray());

    private struct hand { 
        internal int[] ranks;
        internal bool flush;
        internal bool straight;
        internal hand(string handString)
        {
            var (rks, sts) = handString.Split(' ').Select(c => ("__234567891JQKA".IndexOf(c.First()), c.Last())).OrderByDescending(t => t.Item1).Unzip();
            ranks = rks.ToArray();
            flush = sts.Distinct().Count() == 1;
            straight = ranks.Distinct().Count() == 5 && ranks[0] - ranks[4] == 4;
        }
    }

    public static IEnumerable<string> BestHands(IEnumerable<string> hands)
    {
        var scoredHands = new List<(long, string)>();
        var scores = new List<long>();
        foreach (var handString in hands)
        {
            var newScore = GetScore(new hand(handString));
            scores.Add(newScore);
            scoredHands.Add((newScore, handString));
        }
        var mx = scores.Max();
        return scoredHands.Where(pr => pr.Item1 == mx).Select(pr => pr.Item2);
    }

    private static long[] FACTORS = new[] { 10_000_000_000, 100_000_000, 1_000_000, 10_000, 100, 1 }; 
    // at most six items (level + possible 5 crucial ranks) are multiplied with factors and summed up to give a comparable score
    private static long GetScore(hand h) =>   
        FACTORS.Zip(GetLevelAndCrucialRanks(h)).Select(pr => pr.First * (long)pr.Second).Sum();
    
    
    private static int[] ACE_TO_5 = new[] { 14, 5, 4, 3, 2 };
    private static List<int> GetLevelAndCrucialRanks(hand h)
    {   
        if (h.ranks.SequenceEqual(ACE_TO_5)) // handle the special straight here
        {
            if (h.flush) return new List<int> { 8, 5 };
            else return new List<int> { 4, 5 };
        }
   
        var (freqs, ranksByFreqsDesc) = h.ranks
            .GroupBy(r => r)
            .OrderByDescending(g => g.Count())
            .ThenByDescending(g => g.Key)
            .Select(g => (g.Count(), g.Key))
            .Unzip();
        
        if (h.flush && h.straight) // straight flush
        {
            return new List<int> { 8, h.ranks[0] };
        }
        else if (freqs[0] == 4) // 4 of a kind
        {
            return new List<int> { 7, ranksByFreqsDesc[0], ranksByFreqsDesc[1] };
        }
        else if (freqs[0] == 3 && freqs[1] == 2) // full house
        {
            return new List<int> { 6, ranksByFreqsDesc[0], ranksByFreqsDesc[1] };
        }
        else if (h.flush) // flush
        {
            var res = new List<int> { 5 };
            res.AddRange(h.ranks);
            return res;
        }
        else if (h.straight) // straight, from ace to 5 already considered
        {
            return new List<int> { 4, h.ranks[0] };
        }
        else if (freqs[0] == 3) // three of a kind
        {
            return new List<int> { 3, ranksByFreqsDesc[0], ranksByFreqsDesc[1], ranksByFreqsDesc[2] };
        }
        else if (freqs[0] == 2 && freqs[1] == 2) // two pairs
        {
            return new List<int> { 2, ranksByFreqsDesc[0], ranksByFreqsDesc[1], ranksByFreqsDesc[2] };
        }
        else if (freqs[0] == 2) // one pair
        {
            return new List<int> { 1, ranksByFreqsDesc[0], ranksByFreqsDesc[1], ranksByFreqsDesc[2], ranksByFreqsDesc[3] };
        }
        else // high card
        {
            var res = new List<int> { 0 };
            res.AddRange(h.ranks);
            return res;
        }
    }
}