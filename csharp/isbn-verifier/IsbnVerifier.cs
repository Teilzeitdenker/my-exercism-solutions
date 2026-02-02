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
                .Where(char.IsLetterOrDigit)
                .Select( (c, i) => (c == 'X' ? 10 : (int) char.GetNumericValue(c)) * (10 - i))
                .Sum()
                % 11 == 0;
    }
}