using System;
using System.Collections.Generic;
using System.Linq;

public enum YachtCategory
{
    Ones           = 1,
    Twos           = 2,
    Threes         = 3,
    Fours          = 4,
    Fives          = 5,
    Sixes          = 6,
    FullHouse      = 7,
    FourOfAKind    = 8,
    LittleStraight = 9,
    BigStraight    = 10,
    Choice         = 11,
    Yacht          = 12,
}

public static class YachtGame
{
    public static int Score(int[] dice, YachtCategory category)
    {
        if ((int)category <= 6) return dice.Where(x => x == (int)category).Count() * (int)category;
        if ((int)category == 7 && IsFullHouse(dice)) return dice.Sum();
        if ((int)category == 8) return Frequencies(dice).Select(kvp => kvp.Value == 4 || kvp.Value == 5 ? 4 * kvp.Key : 0).Sum();
        if ((int)category == 9 && IsStraight(dice) && dice.Contains(1)) return 30;
        if ((int)category == 10 && IsStraight(dice) && dice.Contains(6)) return 30;
        if ((int)category == 11) return dice.Sum();
        if ((int)category == 12 && dice.Distinct().Count() == 1) return 50;
        return 0;
    }

    static bool IsFullHouse(int[] dice)
    {
        List<int> fhFreqs = new List<int> { 2, 3 };
        return Frequencies(dice).Select(x => x.Value).OrderBy(x => x).ToList().SequenceEqual(fhFreqs);
    }

    static bool IsStraight(int[] dice)
    {
        return dice.Distinct().Count() == 5 && (!dice.Contains(1) || !dice.Contains(6));
    }

    static Dictionary<int, int> Frequencies(int[] dice)
    {
        return dice.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
    }
}

