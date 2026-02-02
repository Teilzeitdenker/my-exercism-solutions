using System;
using System.Linq;
using System.Collections.Generic;
public static class RunLengthEncoding
{
    public static string Encode(string input)
    {
        if (input.Length == 0) return "";
        List<(char, int)> tuples = new List<(char, int)>();
        (char, int) get_next_tuple((char, int) last_count, char actual)
        {
            char last = last_count.Item1;
            int count = last_count.Item2;
            if (last == actual)
            {
                return (last, count + 1);
            } 
            else
            {
                tuples.Add((last, count));
                return (actual, 1);
            }
        }
        var last_tuple = (input.Skip(1).Aggregate((input[0], 1), get_next_tuple));
        tuples.Add(last_tuple);
        return tuples.Aggregate(string.Empty, (s, tuple) =>
                tuple.Item2 > 1 ? s + tuple.Item2.ToString() + tuple.Item1.ToString() : s + tuple.Item1.ToString()
            );
    }

    public static string Decode(string input)
    {
        if (input.Length == 0) return "";
        List<string> chargroups = new List<string>();
        (int, char) get_next_group((int, char) count_before, char actual)
        {
            int count = count_before.Item1;
            char before = count_before.Item2;
            if (count > 0 && !char.IsDigit(actual))
            {
                chargroups.Add(new string(actual, count));
                return (0, actual);
            }
            else if (count > 0 && char.IsDigit(actual))
            {
                return (count * 10 + int.Parse(actual.ToString()), actual);
            }
            else if (count == 0 && !char.IsDigit(actual))
            {
                chargroups.Add(actual.ToString());
                return (0, actual);
            }
            else if (count == 0 && char.IsDigit(actual))
            {
                return (int.Parse(actual.ToString()), actual);
            } else { throw new ArgumentException("Bad argument in get_next_group"); }
        }
        var _last_tuple = (input.Aggregate((0, ' '), get_next_group));
        return String.Join("", chargroups);
    }
}
