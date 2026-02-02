using System;
using System.Collections.Generic;
using System.Linq;

public static class Minesweeper
{
    public static string[] Annotate(string[] input)
    {
        if (input.Length == 0 || input[0] == "" || input.All(line => !line.Contains("*")))
        {
            return input;
        }
        List<(int, int)> MineLocations = GetMineLocations(input);
        return input
               .Select((line, i) => string.Join( "",
                    line
                    .Select((ch, j) => ch == '*' ? -1 : NeighbouringFields(i, j).Where(loc => MineLocations.Contains(loc)).Count())
                    .Select( num => num == -1 ? "*" : num == 0 ? " " : num.ToString() )
                   )
               ).ToArray();
    }

    static List<(int, int)> GetMineLocations(string[] input)
    {
        return input
               .SelectMany(
                    (line, i) => 
                        line
                        .Select((ch, j) => (ch, j))
                        .Where(t => t.ch == '*')
                        .Select(t => (i, t.j))
               ).ToList();
    }   

    static List<(int, int)> NeighbouringFields(int i, int j)
    {
        return new List<(int, int)>
        {
            (i - 1, j - 1),
            (i - 1, j),
            (i - 1, j + 1),
            (i, j - 1),
            (i, j + 1),
            (i + 1, j - 1),
            (i + 1, j),
            (i + 1, j + 1)
        };
    }
}
