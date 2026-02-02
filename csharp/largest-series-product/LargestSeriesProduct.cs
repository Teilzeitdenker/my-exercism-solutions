using System;
using System.Linq;

public static class LargestSeriesProduct
{
    public static long GetLargestProduct(string digits, int span) 
    {
        if (span < 0) throw new ArgumentException();
        if (span == 0) return 1L;
        if (digits.Length == 0) throw new ArgumentException();
        if (digits.Where(c => !Char.IsDigit(c)).Count() > 0) throw new ArgumentException();
        if (span > digits.Length) throw new ArgumentException();
        return Enumerable.Range(0, digits.Length - span + 1)
                    .Select( n => digits
                                    .Substring(n, span)
                                    .Select(c => Int64.Parse(c.ToString()))
                                    .Aggregate( (a, b) => a * b )
                            )
                    .Max();
    }
}