using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class NthPrime
{
    private static int limit = 105_000;
    private static int[] Primes = primesSoE(limit).ToArray();
    static IEnumerable<int> primesSoE(int top_number)
    {
        if (top_number < 2) yield break;
        yield return 2;
        if (top_number < 3) yield break;
        // limit for the bit-array buffer that represents only the odd numbers 
        var BFLMT = (top_number - 3) / 2;
        var SQRTLMT = ((int)(Math.Sqrt((double)top_number)) - 3) / 2;
        var buf = new BitArray((int)BFLMT + 1, true);
        for (int i = 0; i <= BFLMT; ++i)
            if (buf[i])
            {
                int p = 3 + 2 * i;
                if (i <= SQRTLMT)
                {
                    for (int j = (p * p - 3) / 2; j <= BFLMT; j += p)
                        buf[j] = false;
                }
                yield return p;
            }
    }
    public static int Prime(int n)
    {
        if (n <= 0) throw new ArgumentOutOfRangeException("only positive integers allowed");
        return Primes[n-1];
    }
}