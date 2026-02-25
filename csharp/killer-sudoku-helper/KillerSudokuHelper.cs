public static class KillerSudokuHelper
{
    public static IEnumerable<int[]> Combinations(int sum, int size, int[] exclude)
    {
        var takeOnly = Enumerable.Range(1, 9).Except(exclude).ToArray();
        var firstCandidates = takeOnly.Where(e => e <= sum).Select(e => new int[] { e });
        return DoCombinations(sum, size, takeOnly, firstCandidates);
    }

    private static IEnumerable<int[]> DoCombinations(int sum, int size, int[] takeOnly, IEnumerable<int[]> candidates)
    {
        if (size == 1) return candidates.Where(c => c.Sum() == sum).Select(c => c.Reverse().ToArray());
        var newCandidates = candidates
            .SelectMany(c => takeOnly.Where(n => n > c[0]).Select(n => (int[])[n, .. c])) // collection expression with spread!
            .Where(c => c.Sum() <= sum);
        return DoCombinations(sum, size - 1, takeOnly, newCandidates);
    }
}