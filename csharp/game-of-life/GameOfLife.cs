using System;
using System.Collections.Generic;
using System.Linq;

public static class GameOfLife
{
    public static int[,] Tick(int[,] matrix)
    {
        var result = new int[matrix.GetLength(0), matrix.GetLength(1)];
        for (var i = 0; i < matrix.GetLength(0); i++)
            for (var j = 0; j < matrix.GetLength(1); j++)
            {   // rules of the game
                result[i, j] = (matrix[i, j], GetNumAliveNeighbors(i, j, matrix)) switch
                {
                    (_, 3) => 1, // stasis for alive cells, reproduction for dead cells, 
                    (1, 2) => 1, // stasis for alive cells
                    _      => 0  // stasis for dead  cells, under- and overpopulation for alive cells
                };
            }
        return result;
    }

    private static int GetValue(int row, int col, int[,] matrix) => // check if coordinates are out of bounds
        (row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix.GetLength(1)) ? matrix[row, col] : 0;

    private static readonly (int, int)[] Offsets = { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1) };

    private static int GetNumAliveNeighbors(int row, int col, int[,] matrix) => 
        Offsets.Aggregate(0, (acc, offset) => acc + GetValue(row + offset.Item1, col + offset.Item2, matrix));
}
