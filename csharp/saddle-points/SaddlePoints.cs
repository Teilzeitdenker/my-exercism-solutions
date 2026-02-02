using System;
using System.Collections.Generic;
using System.Linq;

public static class SaddlePoints
{
    public static IEnumerable<(int, int)> Calculate(int[,] matrix)
    {
        int numRows = matrix.GetLength(0);
        int numCols = matrix.GetLength(1);
        
        bool isSaddlePoint((int, int) indices)
        {
            int value = matrix[indices.Item1, indices.Item2];
            return Enumerable.Range(0, numCols)
                .All(x => matrix[indices.Item1, x] <= value) 
                && 
                Enumerable.Range(0, numRows)
                .All(x => matrix[x, indices.Item2] >= value);
        }

        return Enumerable.Range(0, numRows)
               .SelectMany(i =>
                   Enumerable.Range(0, numCols)
                   .Select(j => (i, j))
                   .Where(ind => isSaddlePoint(ind))
                )
               .Select(ind => (ind.Item1 + 1, ind.Item2 + 1))
               .ToArray();
    }
}
