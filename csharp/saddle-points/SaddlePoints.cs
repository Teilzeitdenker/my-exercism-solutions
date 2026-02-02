using System;
using System.Collections.Generic;
using System.Linq;

public static class SaddlePoints
{
    public static IEnumerable<(int, int)> Calculate(int[,] matrix)
    {
        int numRows = matrix.GetLength(0);
        int numCols = matrix.GetLength(1);
        bool hasLeftNeighbor((int, int) indices)
        {
            return !(indices.Item2 == 0);
        }
        bool hasRightNeighbor((int, int) indices)
        {
            return !(indices.Item2 == numCols - 1);
        }
        bool hasUpperNeighbor((int, int) indices)
        {
            return !(indices.Item1 == 0);
        }
        bool hasLowerNeighbor((int, int) indices)
        {
            return !(indices.Item1 == numRows - 1);
        }
        bool isSaddlePoint((int, int) indices)
        {
            int value = matrix[indices.Item1, indices.Item2];
            if (hasLeftNeighbor(indices)) { if (matrix[indices.Item1, indices.Item2 - 1] > value) return false; }
            if (hasRightNeighbor(indices)) { if (matrix[indices.Item1, indices.Item2 + 1] > value) return false; }
            if (hasUpperNeighbor(indices)) { if (matrix[indices.Item1 - 1, indices.Item2] < value) return false; }
            if (hasLowerNeighbor(indices)) { if (matrix[indices.Item1 + 1, indices.Item2] < value) return false; }
            return true;
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
