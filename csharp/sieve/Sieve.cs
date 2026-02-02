using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Sieve
{
    public static int[] Primes(int limit)
    {
        if (limit < 0) throw new ArgumentOutOfRangeException();
        return primesSoE(limit).ToArray();
    }

    static IEnumerable<int> primesSoE(int top_number)
    {
        if (top_number < 2) yield break;
        // give back two
        yield return 2; 
        if (top_number < 3) yield break;
        // limit for the bit-array buffer that represents only the odd numbers 
        var BFLMT = (top_number - 3) / 2;
        var SQRTLMT = ((int)(Math.Sqrt((double)top_number)) - 3) / 2;
        // check all bits as marked in the beginning
        var buf = new BitArray((int)BFLMT + 1, true);
        for (int i = 0; i <= BFLMT; ++i) 
            if (buf[i])
            {
                // true at index i represents the prime number 2i + 3
                int p = 3 + 2 * i; 
                if (i <= SQRTLMT)
                {
                    // set multiples to false
                    // since we already have unmarked all nonprimes lower than p^2 we can start at a higher index
                    for (int j = (p * p - 3) / 2; j <= BFLMT; j += p)
                        buf[j] = false;
                }
                yield return p;
            }
    }
}