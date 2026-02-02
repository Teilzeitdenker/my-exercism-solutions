using System;
using System.Collections.Generic;
using System.Linq;

public static class BookStore
{
    public static decimal Total(IEnumerable<int> books)
    {
        if (books.Count() == 0) return 0.0m;
        var numBooksGrouped = books 
            .GroupBy(x => x)
            .Select(g => g.Count())
            .OrderBy(x => x)
            .GroupBy(x => x)
            .Select(g => ( g.Key, g.Count()));
        int actualLength = numBooksGrouped.Select(t => t.Item2).Sum();
        int actualNumber = 0;
        var stacks = numBooksGrouped
            .Aggregate(new List<int>(), (list, group) =>
                {
                    list.AddRange(Enumerable.Repeat(actualLength, count: group.Item1 - actualNumber));
                    actualNumber = group.Item1;
                    actualLength -= group.Item2;
                    return list;

                });
        var stackNumbersGrouped = stacks
            .GroupBy(s => s)
            .Select(g => (g.Key, g.Count()));
        var numbers = Enumerable.Range(1, 5).Select(i => stackNumbersGrouped.FirstOrDefault(g => g.Item1 == i).Item2).ToList();
        var eliminate = numbers[4] > numbers[2] ? numbers[2] : numbers[4];
        var anzahlen = new List<int>
        {
            numbers[0],
            numbers[1],
            numbers[2] - eliminate,
            numbers[3] + 2 * eliminate,
            numbers[4] - eliminate
        };
        return anzahlen
                .Select((el, i) => (i + 1, el))
                .Select(t => priceForStackSize[t.Item1] * t.Item2)
                .Sum();
    }

    private static Dictionary<int, decimal> priceForStackSize = new Dictionary<int, decimal>()
    {
        { 1, 8.0m },
        { 2, 15.2m },
        { 3, 21.6m },
        { 4, 25.6m },
        { 5, 30.0m }
    };
}