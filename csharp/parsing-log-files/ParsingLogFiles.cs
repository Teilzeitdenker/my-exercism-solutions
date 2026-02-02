using System;
using System.Text.RegularExpressions;

public class LogParser
{
    public bool IsValidLine(string text)
    {
        string pattern = @"(^\[TRC\]|^\[DBG\]|^\[INF\]|^\[WRN\]|^\[ERR\]|^\[FTL\])";
        return Regex.IsMatch(text, pattern);
        
    }

    public string[] SplitLogLine(string text)
    {
        string pattern = "<[-*^=]+>";
        return Regex.Split(text, pattern);
    }

    public int CountQuotedPasswords(string lines)
    {
        string splitPattern = "\n";
        string[] l = Regex.Split(lines, splitPattern);
        string matchPattern = @"[""](.)*password(.)*[""]";
        int count = 0;
        foreach (string line in l)
        {
            if (Regex.IsMatch(line, matchPattern, RegexOptions.IgnoreCase)) {
                count++;
            }
        }
        return count;
    }

    public string RemoveEndOfLineText(string line)
    {
        string pattern = @"end-of-line[\d]+";
        return Regex.Replace(line, pattern, String.Empty);
    }

    public string[] ListLinesWithPasswords(string[] lines)
    {
        var result = new string[lines.Length];
        string pattern = @"password[^\s]+";
        for (int i = 0; i < lines.Length; i++)
        {
            string prefix;
            Match m = Regex.Match(lines[i], pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                prefix = m.Value + ": ";
            } else
            {
                prefix = "--------: ";
            }
            result[i] = prefix + lines[i];
        }
        return result;
    }
}
