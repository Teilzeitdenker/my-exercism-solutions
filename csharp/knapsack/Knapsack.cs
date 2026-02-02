using System;
public static class Knapsack
{
    public static int MaximumValue(int mx, (int weight, int value)[] items)
    {
        var dp = new int[mx + 1];
        foreach (var (w, v) in items)
            for (var i = mx; i >= w; i--)
                dp[i] = Math.Max(dp[i], dp[i - w] + v);
        return dp[mx];
    }
}
