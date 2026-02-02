using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public static class IsbnVerifier
{
    static Regex VALID_ISBN = new Regex(@"^(\d-?){9}(\d|X)$");
    public static bool IsValid(string number)
    {
        if (!VALID_ISBN.IsMatch(number)) return false;
        return number
                .Where(c => char.IsDigit(c) || c == 'X')
                .Reverse()
                .Zip(Enumerable.Range(1, 10))
                .Aggregate(
                    0, (acc, tuple) => 
                       acc + ( tuple.First == 'X' ? 10 : (int) char.GetNumericValue(tuple.First) ) * tuple.Second
                    ) 
                % 11 == 0;
    }
}