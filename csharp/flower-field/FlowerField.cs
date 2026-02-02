public static class FlowerField
{
    public static string[] Annotate(string[] input)
    {
        var flowers = FindFlowers(input);
        return input
        .Select((line, i) => new string(line.Select((ch, j) => ch == '*' ? '*' : CountNeighboringFlowers(i, j, flowers)).ToArray()))
        .ToArray();
    }

    private static List<(int row, int col)> FindFlowers(string[] input) =>
    input.SelectMany((line, row) => line.Select((ch, col) => (ch, row, col)).Where(x => x.ch == '*').Select(x => (x.row, x.col))).ToList();

    private static char CountNeighboringFlowers(int i, int j, List<(int i, int j)> flowers)
    {
        var neighbors = new List<(int i, int j)>
        {   // 8 neighboring positions, the missing field is the position itself
            (i - 1, j - 1), (i - 1, j), (i - 1, j + 1),
            (i    , j - 1),             (i    , j + 1),
            (i + 1, j - 1), (i + 1, j), (i + 1, j + 1)
        };
        var count = neighbors.Count(n => flowers.Contains(n));
        var count_char = (char)('0' + count);
        return count == 0 ? ' ' : count_char;
    }
}
