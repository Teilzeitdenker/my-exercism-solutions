using System;
using System.Collections.Generic;
using System.Linq;

public enum ConnectWinner { White, Black, None }

public class Connect
{
    private readonly char[] ALLOWED = new[] {'.', 'O', 'X'};
    private readonly int _height;
    private readonly int _width;
    private readonly char[,] _grid;
    private readonly List<(int, int)> _whiteStartFields = new();
    private readonly List<(int, int)> _blackStartFields = new();
    private readonly List<(int, int)> _whiteEndFields = new();
    private readonly List<(int, int)> _blackEndFields = new();
    

    public Connect(string[] input)
    {
        _height = input.Length;
        if (_height == 0) throw new ArgumentException("input is an empty array");
        
        _width = (input[0].Length + 1) / 2;
        if (_width == 0) throw new ArgumentException("first line is an empty string");
        
        var gridRows = input.Select(line => line.TrimStart()).ToArray();
        foreach (var row in gridRows) 
            if (row.Length != 2 * _width - 1)
                throw new ArgumentException("inconsistent row lengths");
        
        _grid = new char[_height, _width]; 
        for (int row = 0; row < _height; row++)
            for (int col = 0; col < _width; col++)
            {
                if (col < _width - 1 && gridRows[row][col * 2 + 1] != ' ') 
                    throw new ArgumentException(
                            $"board entries in line {row + 1} must be seperated by whitespace"
                        );

                if (!ALLOWED.Contains(gridRows[row][col * 2]))
                    throw new ArgumentException($"unallowed board entry in line {row + 1} of input");
                
                _grid[row, col] = gridRows[row][col * 2];
            }
                
        for (int col = 0; col < _width; col++)
        {
            if (_grid[0, col] == 'O') _whiteStartFields.Add((0, col));
            if (_grid[_height - 1, col] == 'O') _whiteEndFields.Add((_height - 1, col));
        }

        for (int row = 0; row < _height; row++)
        {
            if (_grid[row, 0] == 'X') _blackStartFields.Add((row, 0));
            if (_grid[row, _width - 1] == 'X') _blackEndFields.Add((row, _width - 1));
        }
    }
    
    public ConnectWinner Result() =>
        hasConnection(white: true) ? ConnectWinner.White : 
            (hasConnection(white: false) ? ConnectWinner.Black : ConnectWinner.None);

    private bool hasConnection(bool white)
    {
        var startFields = white ? _whiteStartFields : _blackStartFields;
        var endFields = white ? _whiteEndFields : _blackEndFields;
        if (startFields.Count == 0 || endFields.Count == 0) return false;
        
        var owner = white ? 'O' : 'X';
        var visited = new bool[_height, _width];
        var q = new Queue<(int, int)>(startFields);
        
        while (q.Count > 0)
        {
            var (x, y) = q.Dequeue();
            if (endFields.Contains((x, y))) return true; // success
            
            if (x > 0 && _grid[x - 1, y] == owner) // up left
                if (!visited[x - 1, y])
                {
                    visited[x - 1, y] = true;
                    q.Enqueue((x - 1, y));
                }
            if (x > 0 && y < _width - 1 && _grid[x - 1, y + 1] == owner) // up right
                if (!visited[x - 1, y + 1])
                {
                    visited[x - 1, y + 1] = true;
                    q.Enqueue((x - 1, y + 1));
                }
            if (y > 0 && _grid[x, y - 1] == owner) // left
                if (!visited[x, y - 1])
                {
                    visited[x, y - 1] = true;
                    q.Enqueue((x, y - 1));
                }
            if (y < _width - 1 && _grid[x, y + 1] == owner) // right
                if (!visited[x, y + 1])
                {
                    visited[x, y + 1] = true;
                    q.Enqueue((x, y + 1));
                }
            if (x < _height - 1 && y > 0 && _grid[x + 1, y - 1] == owner) // down left
                if (!visited[x + 1, y - 1])
                {
                    visited[x + 1, y - 1] = true;
                    q.Enqueue((x + 1, y - 1));
                }
            if (x < _height - 1 && _grid[x + 1, y] == owner) // down right
                if (!visited[x + 1, y])
                {
                    visited[x + 1, y] = true;
                    q.Enqueue((x + 1, y));
                }
        }
        return false;
    }
}