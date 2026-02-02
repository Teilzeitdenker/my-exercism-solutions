using System;
using System.Collections.Generic;

public class PhoneNumber
{
    public static string Clean(string phoneNumber)
    {
        string s = String.Empty;
        foreach (char c in phoneNumber)
        {
            if (Char.IsDigit(c)) s += c;
        }
        if (s.Length == 10)
        {
            if (s[0] == '0' || s[0] == '1' || s[3] == '0' || s[3] == '1') throw new ArgumentException();
            else return s;
        }
        if (s.Length == 11)
        {
            if (s[0] == '1') return PhoneNumber.Clean(s.Substring(1));
            else throw new ArgumentException();
        }
        else throw new ArgumentException();
    }
    
}