using System;
using System.Collections.Generic;
using System.Linq;

public static class PalindromeProducts
{
    public static (int, IEnumerable<(int,int)>) Largest(int minFactor, int maxFactor)
    {
        List<(int, int)> factors = new List<(int, int)>();
        int max = 0;
        for (int i = minFactor; i <= maxFactor; i++)
        {
            for (int j = i; j <= maxFactor; j++)
            {
                if (i * j > max && isPalindrome(i * j))
                {
                    max = i * j;
                    factors = new List<(int, int)> { (i, j) };
                }
                else if (i * j == max)
                {
                    factors.Add((i, j));
                }
            }
        }
        if (factors.Count == 0) throw new ArgumentException("No palindrome found");
        return (max, factors);
    }

    public static (int, IEnumerable<(int,int)>) Smallest(int minFactor, int maxFactor)
    {
        List<(int, int)> factors = new List<(int, int)>();
        int min = maxFactor * maxFactor + 1;
        for (int i = minFactor; i <= maxFactor; i++)
        {
            for (int j = i; j <= maxFactor; j++)
            {
                if (i * j < min && isPalindrome(i * j))
                {
                    min = i * j;
                    factors = new List<(int, int)> { (i, j) };
                }
                else if (i * j == min)
                {
                    factors.Add((i, j));
                }
            }
        }
        if (factors.Count == 0) throw new ArgumentException("No palindrome found");
        return (min, factors);
    }
    private static bool isPalindrome(int n)
    {
        string s = n.ToString();
        int len = s.Length;
        for (int i = 0; i < len / 2; i++)
        {
            if (s[i] != s[len - 1 - i]) return false;
        }
        return true;
    }
}
