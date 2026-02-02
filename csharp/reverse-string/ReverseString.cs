using System;
using System.Collections.Generic;

public static class ReverseString
{
    public static string Reverse(string input)
    {
        string result = String.Empty;
        for (int i = input.Length; i != 0; i--)
        {
            result += input[i - 1];
        }
        return result;
    }
}