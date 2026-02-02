using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class SumOfMultiples
{
    public static int Sum(IEnumerable<int> multiples, int max)
    {
        return Enumerable.Range(1, max-1).Where(i => IsMultiple(multiples, i)).Sum();
    }
    private static bool IsMultiple(IEnumerable<int> multiples, int n)
    {
        foreach (int cand in multiples)
        {
            if (cand != 0 && n % cand == 0) return true;
        }
        return false;
    }
}