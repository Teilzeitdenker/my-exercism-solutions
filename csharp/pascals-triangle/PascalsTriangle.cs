using System;
using System.Linq;
using System.Collections.Generic;

public static class PascalsTriangle
{
    public static IEnumerable<IEnumerable<int>> Calculate(int rows)
    {
        if (rows == 0) return Calculate(1).Where(arr => arr == new int[] { });
        if (rows == 1) return new[] { new int[] { 1 } };
        IEnumerable<IEnumerable<int>> before = Calculate(rows - 1);
        IEnumerable<int> toAppend = before.Last().Zip(before.Last().Skip(1)).Select(a => a.First + a.Second);
        return before.Append(toAppend.Prepend(1).Append(1));
    }

}