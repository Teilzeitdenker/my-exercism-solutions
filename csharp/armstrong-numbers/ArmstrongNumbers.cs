using System;
using System.Collections.Generic;
using System.Linq;

public static class ArmstrongNumbers
{
    public static bool IsArmstrongNumber(int number)
    {
        int result = 0;
        double exponent = Math.Floor(Math.Log10(number) + 1);
        foreach (int digit in DigitsReversed(number))
        {
            result += (int)Math.Pow(digit, exponent);
        }
        return result == number;
    }
    private static IEnumerable<int> DigitsReversed(int number)
    {
        while (number != 0)
        {
            yield return number % 10;
            number /= 10;
        }
    }
}