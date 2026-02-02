using System;
using System.Collections.Generic;

public static class PrimeFactors
{
    private static bool[] _sieve = new bool[0];
    public static long[] Factors(long number)
    {
        if (number <= 1L) return new long[0];
        List<long> primes = new List<long>();
        while (number % 2L == 0)
        {
            primes.Add(2L);
            number /= 2L;
        }
        for (long factor = 3L; factor <= number; factor += 2L)
        {
            while (number % factor == 0)
            {
                primes.Add(factor);
                number /= factor;
            }
        }
        return primes.ToArray();
    }
}