using System;
using System.Linq;

public static class AllYourBase
{
    public static int[] Rebase(int inputBase, int[] inputDigits, int outputBase)
    {
        if (inputBase <= 1 || outputBase <= 1 || inputDigits.Where(d => d < 0 || d >= inputBase).Count() >= 1) throw new ArgumentException();
        int actual = inputDigits.Reverse().Select((d, i) => d * (int)Math.Pow(inputBase, i)).Sum();
        if (actual == 0) return new int[] { 0 };
        int highestExponent = (int)Math.Floor(Math.Log2(actual) / Math.Log2(outputBase)) + 1;
        return Enumerable.Range(0, highestExponent).Reverse().Select(i => ( actual / (int)Math.Pow(outputBase, i) ) % outputBase ).ToArray();
    }
}