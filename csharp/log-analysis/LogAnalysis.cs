using System;

public static class LogAnalysis 
{
    public static string SubstringAfter(this string str, string delimiter)
    {
        int start = str.IndexOf(delimiter) + delimiter.Length;
        return str.Substring(start);
    }

    public static string SubstringBetween(this string str, string del1, string del2)
    {
        int start = str.IndexOf(del1) + del1.Length;
        int end = str.IndexOf(del2);
        return str.Substring(start, end - start);
    }

    public static string Message(this string str)
    {
        return str.SubstringAfter(": ");
    }

    public static string LogLevel(this string str)
    {
        return str.SubstringBetween("[", "]");
    }
}