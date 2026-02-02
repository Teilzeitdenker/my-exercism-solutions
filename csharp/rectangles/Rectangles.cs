using System;
using System.Linq;
using System.Collections.Generic;

public static class Rectangles
{
    public static int Count(string[] rows)
    {
        if (rows.Length == 0 || rows.Where(row => row.Contains('+')).Count() == 0) return 0;
        var cornerPairs = GetCornerPairs();
        var transpose = TransposeInput();
        var horizontal = new char[] { '+', '-' };
        var vertical = new char[] { '+', '|' };

        return cornerPairs
            .Select((pairs, row1) =>
                pairs.Select(pair => CountRectanglesForPair(pair, row1)).Sum())
            .Sum();

        // Local functions

        List<List<(int, int)>> GetCornerPairs()
        {
            var result = new List<List<(int, int)>>();
            foreach (string row in rows)
            {
                var indices = row.Select((c, i) => (c, i)).Where(el => el.c == '+').Select(el => el.i).ToArray();
                var cornerPairs = new List<(int, int)>();
                for (var i = 0; i < indices.Length - 1; i++)
                {
                    for (var j = i + 1; j < indices.Length; j++)
                    {
                        cornerPairs.Add((indices[i], indices[j]));
                    }
                }
                result.Add(cornerPairs);
            }
            return result;
        }
        
        string[] TransposeInput()
        {
            var transpose = new string[rows[0].Length];
            for (var i = 0; i < transpose.Length; i++)
            {
                var line = new string("");
                for (var j = 0; j < rows.Length; j++)
                {
                    line += rows[j][i];
                }
                transpose[i] = line;
            }
            return transpose;
        }

        int CountRectanglesForPair((int, int) pair, int row1)
        {
            return cornerPairs
                .Select((pairs, row2) => (pairs, row2))
                .Skip(row1 + 1)
                .Where(el => el.pairs.Contains(pair) && CheckRectangle(row1, el.row2, pair))
                .Count();
        }

        bool CheckRectangle(int row1, int row2, (int, int) columns)
        {
            int col1 = columns.Item1;
            int col2 = columns.Item2;
            return rows[row1].Substring(col1 + 1, col2 - col1 - 1).All(c => horizontal.Contains(c))
                && rows[row2].Substring(col1 + 1, col2 - col1 - 1).All(c => horizontal.Contains(c))
                && transpose[col1].Substring(row1 + 1, row2 - row1 - 1).All(c => vertical.Contains(c))
                && transpose[col2].Substring(row1 + 1, row2 - row1 - 1).All(c => vertical.Contains(c));
        }
    }
}