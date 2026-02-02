using System;
using System.Linq;


public static class Luhn
{
    public static bool IsValid(string number)
    {
        if (number.Where(c => !char.IsWhiteSpace(c)).Any(c => !char.IsDigit(c))) return false;

        int DoubleIt(int d)
        {
            if (2 * d > 9) return 2 * d - 9;
            return 2 * d;
        }

        var digits = number
                .Where(char.IsDigit);
        
        if (digits.Count() <= 1) return false;
        
        return digits 
                .Select(c => int.Parse(c.ToString()))
                .Reverse()
                .Select((n, i) => i % 2 == 0 ? n : DoubleIt(n))
                .Sum() % 10 == 0;
     
    }

}