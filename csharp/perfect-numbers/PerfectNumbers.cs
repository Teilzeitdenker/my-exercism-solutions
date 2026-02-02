using System;
using System.Collections.Generic;
using System.Linq;

public enum Classification
{
    Perfect,
    Abundant,
    Deficient
}

public static class PerfectNumbers
{
    public static Classification Classify(int number)
    {
        if (number <= 0) throw new ArgumentOutOfRangeException();
        return DivisorSum(number).CompareTo(number) switch
        {
            0 => Classification.Perfect,
            1 => Classification.Abundant,
            -1 => Classification.Deficient,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static int DivisorSum(int n)
    {
        return Enumerable.Range(1, n / 2).Where(a => n % a == 0).Select(a => a).Sum();
    }
}
