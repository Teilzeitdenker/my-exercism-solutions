using System;
using System.Diagnostics.CodeAnalysis;

public static class Say
{
    private static string[] Units = new[]
    {
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven",
        "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
    };
    private static string[] Tens = new[]
    {
        "_", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
    };
    private static string SayIt(long number, string fillChar)
    {
        return number switch
        {
            0L                             => "",
            long n when n < 20L            => fillChar + Units[(int)n],
            long n when n < 100L           => fillChar + Tens[(int)(n/10L)]                          + SayIt(n%10L, "-"),
            long n when n < 1_000L         => fillChar + SayIt(n/100L, "")             + " hundred"  + SayIt(n%100L, " "),
            long n when n < 1_000_000L     => fillChar + SayIt(n / 1_000L, "")         + " thousand" + SayIt(n % 1_000L, " "),
            long n when n < 1_000_000_000L => fillChar + SayIt(n / 1_000_000L, "")     + " million"  + SayIt(n % 1_000_000L, " "),
            long n                         => fillChar + SayIt(n / 1_000_000_000L, "") + " billion"  + SayIt(n % 1_000_000_000L, " ")
        };
    }
    public static string InEnglish(long number)
    {
        if (number < 0L || number >= 1_000_000_000_000L) throw new ArgumentOutOfRangeException("number must be in the range 0..999_999_999_999");
        if (number == 0L) return "zero";
        return SayIt(number, "");
    }
}