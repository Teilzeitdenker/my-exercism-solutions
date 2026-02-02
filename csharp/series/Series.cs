using System;
using System.Collections.Generic;

public static class Series
{
    public static string[] Slices(string numbers, int sliceLength)
    {
        int l = numbers.Length;
        if (sliceLength < 1 || sliceLength > l) throw new ArgumentException();
        var result = new List<string>();
        for (int i = 0; i <= l - sliceLength; i++)
        {
            result.Add(numbers.Substring(i, sliceLength));
        }
        return result.ToArray();
    }
}