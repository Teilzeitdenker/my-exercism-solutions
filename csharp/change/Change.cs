using System;
using System.Collections.Generic;

public static class Change
{
    private const int INF = Int32.MaxValue - 1;
    public static int[] FindFewestCoins(int[] coins, int target)
    {
        if (target == 0) return new int[0];
        Array.Sort(coins);
        if (coins.Length == 0 || target < 0 || target < coins[0])
        {
            throw new ArgumentException("invalid input");
        }
        int[] nextCoin = new int[target + 1];
        int[] numCoins = new int[target + 1];
        Array.Fill(numCoins, INF);
        numCoins[0] = 0;
        for (int x = 1; x <= target; x++)
        {
            foreach (var c in coins)
            {
                if (x - c >= 0 && numCoins[x - c] + 1 < numCoins[x])
                {
                    numCoins[x] = numCoins[x - c] + 1;
                    nextCoin[x] = c;
                }
            }
        }
        if (numCoins[target] == INF) throw new ArgumentException("no possible solution");
        var result = new int[numCoins[target]];
        int idx = 0;
        while (idx < result.Length)
        {
            int nxt = nextCoin[target];
            result[idx] = nxt;
            target -= nxt; // diminish the target value by this coin value
            idx++;
        }
        return result;
    }
}