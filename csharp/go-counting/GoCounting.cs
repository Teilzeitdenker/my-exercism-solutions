using System;
using System.Collections.Generic;
using System.Linq;

public enum Owner { None, Black, White }

public class GoCounting
{
    private string[] _board;

    public GoCounting(string input) => _board = input.Split('\n');

    public (Owner, HashSet<(int, int)>) Territory((int, int) c)
    {
        if (!OnBoard(c)) throw new ArgumentException("out of range");
        if (OwnerOf(c) != Owner.None) return (Owner.None, new());

        var (fields, edges) = (new HashSet<(int, int)> { c }, new HashSet<(int, int)>());
        var neighbors = Neighbors(c).ToArray();

        while (neighbors.Any())
        {
            foreach (var n in neighbors)
            {
                if (OwnerOf(n) == Owner.None) fields.Add(n);
                else edges.Add(n);
            }

            neighbors = neighbors
                .Where(n => OwnerOf(n) == Owner.None)
                .SelectMany(Neighbors)
                .Where(n => !fields.Contains(n) && !edges.Contains(n)).ToArray();
        }

        if (edges.Select(OwnerOf).Distinct().Count() == 1) 
             return (OwnerOf(edges.First()), fields);

        else return (Owner.None, fields);
    }

    public Dictionary<Owner, HashSet<(int, int)>> Territories()
    {
        var res = new Dictionary<Owner, HashSet<(int, int)>>
        {
            [Owner.None] = new(), [Owner.White] = new(), [Owner.Black] = new()
        };
        var visited = new bool[_board[0].Length, _board.Length];

        for (int x = 0; x < _board[0].Length; x++)
            for (int y = 0; y < _board.Length; y++)      
                if (!visited[x, y])
                {
                    visited[x, y] = true;
                    var (owner, territory) = Territory((x, y));

                    foreach (var (cx, cy) in territory)
                    {
                        res[owner].Add((cx, cy));
                        visited[cx, cy] = true;
                    }
                }  

        return res;
    }

    private (int, int)[] DELTAS = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

    private IEnumerable<(int, int)> Neighbors((int, int) c) => 
        DELTAS.Select(d => (c.Item1 + d.Item1, c.Item2 + d.Item2)).Where(OnBoard);

    private bool OnBoard((int x, int y) c) => 
        c.x >= 0 && c.y >= 0 && c.x < _board[0].Length && c.y < _board.Length;

    private Owner OwnerOf((int x, int y) c) =>  _board[c.y][c.x] == 'W' ? Owner.White :
        _board[c.y][c.x] == 'B' ? Owner.Black : Owner.None;
}
