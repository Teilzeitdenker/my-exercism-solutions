public static class KillerSudokuHelper
{
    public static IEnumerable<int[]> Combinations(int sum, int size, int[] exclude) =>
        CombinationsWithoutRepetition(Enumerable.Range(1, 9).Except(exclude).ToArray(), size)
        .Where(c => c.Sum() == sum);

    private static IEnumerable<int[]> CombinationsWithoutRepetition(int[] numbers, int size) => 
        size == 1 ? numbers.Select(n => new int[] { n }) 
        : numbers
            .SelectMany(n => CombinationsWithoutRepetition(numbers.Where(x => x > n).ToArray(), size - 1)
            .Select(c => (int[])[n, .. c])); // collection expression with spread operator
}