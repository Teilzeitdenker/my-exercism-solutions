using System;
using System.Collections.Generic;
using System.Linq;

public static class RomanNumeralExtension
{
    static Dictionary<int, string> _dictionary = new Dictionary<int, string>
    {
        {1000, "M"},
        {900, "CM"},
        {500, "D"},
        {400, "CD"},
        {100, "C"},
        {90, "XC"},
        {50, "L"},
        {40, "XL"},
        {10, "X"},
        {9, "IX"},
        {5, "V"},
        {4, "IV"},
        {1, "I"},
    };
    public static string ToRoman(this int value)
    {
        if (value < 0 || value >= 4000)
        {
            throw new ArgumentOutOfRangeException("number cannot be converted to roman");
        }
        string roman = "";
        while (value > 0)
        {
            var entry = _dictionary.Where(kv => kv.Key <= value).First();
            roman += entry.Value;
            value -= entry.Key;
        }
        return roman;
    }
}