using System;
using System.Collections.Generic;

public static class ScrabbleScore
{
    private static List<char> one_point = new List<char> { 'A', 'E', 'I', 'O', 'U', 'L', 'N', 'R', 'S', 'T' };
    private static List<char> two_points = new List<char> { 'D', 'G' };
    private static List<char> three_points = new List<char> { 'B', 'C', 'M', 'P' };
    private static List<char> four_points = new List<char> { 'F', 'H', 'V', 'W', 'Y' };
    private static List<char> five_points = new List<char> { 'K' };
    private static List<char> eight_points = new List<char> { 'J', 'X' };
    public static int Score(string input)
    {
        int score = 0;
        foreach (char c in input.ToUpper())
        {
            if (one_point.Contains(c)) score += 1;
            else if (two_points.Contains(c)) score += 2;
            else if (three_points.Contains(c)) score += 3;
            else if (four_points.Contains(c)) score += 4;
            else if (five_points.Contains(c)) score += 5;
            else if (eight_points.Contains(c)) score += 8;
            else score += 10;
        }
        return score;
    }
}