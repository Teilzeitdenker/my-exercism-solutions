using System;
using System.Linq;
using System.Collections.Generic;

public static class Poker
{
    // extension method to unzip IEnumerables of tuples (T1, T2) again (here already converted to arrays of the kinds T1 and T2)
    // call it with "var (fst, snd) = EnumerableToUnzip.Unzip(); " 
    private static (T1[], T2[]) Unzip<T1, T2>(this IEnumerable<(T1, T2)> src) => (src.Select(t => t.Item1).ToArray(), src.Select(t => t.Item2).ToArray());
    
    // only have to know the ranks and if it is a flush or a straight, so precalculate these when parsing the deal
    private struct hand { 
        internal int[] ranks; 
        internal bool flush;
        internal bool straight;
        internal hand(string deal)
        {
            // calling the unzip method to get ranks and suits separately (can leave the suits as characters, only interesting for flush)
            var (rks, sts) = deal
                .Split(' ')
                .Select(c => ("__234567891JQKA".IndexOf(c.First()), c.Last()))
                .OrderByDescending(t => t.Item1)
                .Unzip();
            ranks = rks;
            flush = sts.Distinct().Count() == 1;
            straight = ranks.Distinct().Count() == 5 && ranks[0] - ranks[4] == 4;
        }
    }

    public static IEnumerable<string> BestHands(IEnumerable<string> hands)
    {
        var scoredDeals = hands.Select(deal => (GetScore(new hand(deal)), deal));
        var mx = scoredDeals.Unzip().Item1.Max(); // calling Unzip() again
        return scoredDeals.Where(tuple => tuple.Item1 == mx).Select(tuple => tuple.Item2);
    }

    private static long[] FACTORS = new[] { 10_000_000_000, 100_000_000, 1_000_000, 10_000, 100, 1 }; 
    // at most six items (level + possible 5 crucial ranks) are multiplied with factors and summed up to give a comparable score
    private static long GetScore(hand h) =>   
        FACTORS.Zip(GetLevelAndCrucialRanks(h)).Select(pr => pr.First * (long)pr.Second).Sum();
    
    private static int[] ACE_TO_5 = new[] { 14, 5, 4, 3, 2 };
    private static IEnumerable<int> GetLevelAndCrucialRanks(hand h)
    {   
        if (h.ranks.SequenceEqual(ACE_TO_5)) // handle the special straight first
        {
            if (h.flush) return new[] { 8, 5 };
            else return new[] { 4, 5 };
        }
        // again use the Unzip() - method to get two separate arrays   
        var (freqs, ranksByFreqsDesc) = h.ranks
            .GroupBy(r => r)
            .OrderByDescending(g => g.Count())
            .ThenByDescending(g => g.Key)
            .Select(g => (g.Count(), g.Key))
            .Unzip();

        return (h.flush, h.straight, freqs[0]) switch
        {
            (true, true, _)                    => new[] { 8, h.ranks[0] },     // straight flush
            (_   , _   , 4)                    => ranksByFreqsDesc.Prepend(7), // 4 of a kind
            (_   , _   , 3) when freqs[1] == 2 => ranksByFreqsDesc.Prepend(6), // full house
            (true, _   , _)                    => h.ranks.Prepend(5),          // flush
            (_   , true, _)                    => new[] { 4, h.ranks[0] },     // straight
            (_   , _   , 3)                    => ranksByFreqsDesc.Prepend(3), // 3 of a kind
            (_   , _   , 2) when freqs[1] == 2 => ranksByFreqsDesc.Prepend(2), // two pairs
            (_   , _   , 2)                    => ranksByFreqsDesc.Prepend(1), // one pair
            _                                  => h.ranks.Prepend(0)           // high card
        };
    }
}