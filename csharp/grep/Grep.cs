using System;
using System.IO;
using System.Text.RegularExpressions;

public static class Grep
{
    static string Format(bool use_filename, bool use_line_number, string filename, int line_number, string val)
    {
        string file = null;
        if (use_filename)
        {
            file = filename + ':';
        } else
        {
            file = string.Empty;
        }
        string num = null;
        if (use_line_number)
        {
            num = line_number.ToString() + ':';
        } else
        {
            num = string.Empty;
        }
        return $"{file}{num}{val}\n";
    }
    public static string Match(string pattern, string flags, string[] files)
    {
        string result = string.Empty;
        string pattern_str = null;
        if (flags.Contains("-x"))
        {
            pattern_str = $"^{pattern}$";
        } else
        {
            pattern_str = pattern;
        }
        Regex rgx = null;
        if (flags.Contains("-i"))
        {
            rgx = new Regex(pattern_str, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        } else
        {
            rgx = new Regex(pattern_str, RegexOptions.Compiled);
        }
        foreach (string file in files)
        {
            using (StreamReader sreader = File.OpenText(file))
            {
                string line = null;
                int line_number = 0;
                while ((line = sreader.ReadLine())!= null)
                {
                    line_number++;
                    if (rgx.IsMatch(line) ^ flags.Contains("-v"))
                    {
                        if (flags.Contains("-l"))
                        {
                            result += file + '\n';
                            break;
                        } else
                        {
                            result += Format(files.Length > 1, flags.Contains("-n"), file, line_number, line);
                        }
                    }
                }
            }
        }
        return result.TrimEnd();
    }
}