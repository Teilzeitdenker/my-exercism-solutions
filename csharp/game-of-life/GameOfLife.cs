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
                var aliveNeighbors = GetNumAliveNeighbors(i, j, matrix);
                var cellAlive = matrix[i, j] == 1 ? true : false;
                if (cellAlive) 
                {
                    if (aliveNeighbors < 2 || aliveNeighbors > 3) result[i, j] = 0; // under- and overpopulation
                    else result[i, j] = 1; // stasis
                } else // cellDead
                {
                    if (aliveNeighbors == 3) result[i, j] = 1; // reproduction
                    else result[i, j] = 0; // stasis
                }
            }
        return result;
    }

    private static int GetValue(int row, int col, int[,] matrix) => // check if coordinates are out of bounds
        (row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix.GetLength(1)) ? matrix[row, col] : 0;

    private static readonly Tuple<int, int>[] Offsets = new Tuple<int, int>[]
    {
        Tuple.Create(-1,  0), Tuple.Create( 1, 0), Tuple.Create(0, -1), Tuple.Create(0, 1), 
        Tuple.Create(-1, -1), Tuple.Create(-1, 1), Tuple.Create(1, -1), Tuple.Create(1, 1)
    };

    private static int GetNumAliveNeighbors(int row, int col, int[,] matrix) => 
        Offsets.Aggregate(0, (acc, offset) => acc + GetValue(row + offset.Item1, col + offset.Item2, matrix));
}
