using System;
using System.Collections.Generic;

public static class PythagoreanTriplet
{
    public static IEnumerable<(int a, int b, int c)> TripletsWithSum(int sum)
    {
        var triplets = new List<(int, int, int)>();
        int upperLimitA = sum / 3;
        for (int a_cand = 1; a_cand <= upperLimitA; a_cand++)
        {
            int upperLimitB = (sum - a_cand) / 2;
            for (int b_cand = a_cand + 1; b_cand <= upperLimitB; b_cand++)
            {
                int c_cand = sum - a_cand - b_cand;
                if (a_cand * a_cand + b_cand * b_cand == c_cand * c_cand) triplets.Add((a_cand, b_cand, c_cand));
            }
        }
        return triplets;
    }
}