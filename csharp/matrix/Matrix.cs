using System;
using System.Linq;

public class Matrix
{
    private int _rowLength;
    private int _columnLength;
    private int[,] _matrix;
    public Matrix(string input)
    {
        string[] rows = input.Split("\n", StringSplitOptions.TrimEntries);
        _columnLength = rows.Length;
        if (_columnLength == 0) throw new ArgumentException("Input string is empty");
        
        _rowLength = rows[0].Split(' ', StringSplitOptions.TrimEntries).Length;
        _matrix = new int[_columnLength, _rowLength];
        
        for (int i = 0; i < _columnLength; ++i)
        {
            int[] row = rows[i].Split(' ', StringSplitOptions.TrimEntries).Select(c => int.Parse(c)).ToArray();
            for (int j = 0; j < _rowLength; ++j)
            {
                _matrix[i, j] = row[j];
            }
        }
    }

    public int[] Row(int row)
    {
        return Enumerable.Range(0, _matrix.GetLength(1)).Select(j => _matrix[row - 1, j]).ToArray();
    }

    public int[] Column(int col)
    {
        return Enumerable.Range(0, _matrix.GetLength(0)).Select(i => _matrix[i, col - 1]).ToArray();
    }   
}
