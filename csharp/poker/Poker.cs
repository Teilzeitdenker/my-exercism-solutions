using System;
using System.Linq;
using System.Collections.Generic;

public static class Poker
{
    public static char[] RANKS = new[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
    public static char[] SUITS = new[] { 'C', 'D', 'H', 'S'};
    public struct hand { 
        public List<(int, int)> cards; // first int is index of rank in RANKS, second is index of suit in SUITS
        public hand(string handString)
        {
            var res = new List<(int, int)> ();
            foreach (var c in handString.Split(' '))
            {
                if (c.Count() == 3) // extra case for rank '10' + suit (silly to not use the usual 'T' encoding in the input strings)
                {
                    res.Add((Array.IndexOf(RANKS, 'T'), Array.IndexOf(SUITS, c[2])));
                }
                else
                {
                    res.Add((Array.IndexOf(RANKS, c[0]), Array.IndexOf(SUITS, c[1])));
                }
            }
            cards = res.OrderByDescending(c => c.Item1).ToList(); // order cards by rank descending
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

    public static long[] FACTORS = new[] { 10_000_000_000, 100_000_000, 1_000_000, 10_000, 100, 1 }; 
    // at most six items (level + possible 5 crucial ranks) are multiplied with factors and summed up to give a nice comparable score
    public static long GetScore(hand h) =>   
        FACTORS.Zip(GetLevelAndCrucialRanks(h)).Select(pr => pr.First * (long)pr.Second).Sum();
    
    
    public static int[] ACE_TO_5 = new[] { 12, 3, 2, 1, 0 };
    public static List<int> GetLevelAndCrucialRanks(hand h)
    {
        // do all cards have the same suit ?
        bool flush = h.cards.Skip(1).All(c => c.Item2 == h.cards[0].Item2);
        
        var ranks = h.cards.Select(c => c.Item1).ToArray();
        if (ranks.SequenceEqual(ACE_TO_5)) // handle the special straight here
        {
            if (flush) return new List<int> { 8, 3 }; // "highest" card in ACE_TO_5 is '5' with an index of 3 in RANKS
            else return new List<int> { 4, 3 };
        }

        // look at the differences of consecutive values to decide if it is a straight
        bool straight = ranks.Zip(ranks.Skip(1)).Select(c => c.First - c.Second).All(d => d == 1); 
        
        // make a list of frequency-rank tuples
        int freq = 1;
        int actual = ranks[0];
        var freqsAndRanks = new List<(int, int)>();
        for (int i = 1; i < ranks.Length; i++)
        {
            if (ranks[i] == actual)
            {
                freq += 1;
                continue;
            }
            else
            {
                freqsAndRanks.Add((freq, actual));
                actual = ranks[i];
                freq = 1;
            }
        }
        freqsAndRanks.Add((freq, actual)); 
        
        // order this 
        freqsAndRanks = freqsAndRanks.OrderByDescending(c => c.Item1).ThenByDescending(c => c.Item2).ToList();
        
        // and extract freqs and freq-sorted ranks
        var freqs = freqsAndRanks.Select(c => c.Item1).ToList();
        var ranksByFreqsDesc = freqsAndRanks.Select(c => c.Item2).ToList();

        // look at all possible cases starting with the best possible level
        // also append all the ranks that may decide in case of a tie
        if (flush && straight) // straight flush
        {
            return new List<int> { 8, ranks[0] };
        }
        else if (freqs[0] == 4) // 4 of a kind
        {
            return new List<int> { 7, ranksByFreqsDesc[0], ranksByFreqsDesc[1] };
        }
        else if (freqs[0] == 3 && freqs[1] == 2) // full house
        {
            return new List<int> { 6, ranksByFreqsDesc[0], ranksByFreqsDesc[1] };
        }
        else if (flush) // flush
        {
            var res = new List<int> { 5 };
            res.AddRange(ranks);
            return res;
        }
        else if (straight) // straight, from ace to 5 already considered
        {
            return new List<int> { 4, ranks[0] };
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
            res.AddRange(ranks);
            return res;
        }
    }
}