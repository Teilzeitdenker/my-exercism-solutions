using System;
using System.Collections.Generic;
using System.Linq;

public enum SublistType
{
    Equal,
    Unequal,
    Superlist,
    Sublist
}

public static class Sublist
{
    public static SublistType Classify<T>(List<T> list1, List<T> list2) where T : IComparable 
        => (list1.Count, list2.Count) switch
        {
            (int a, int b) when a < b => IsSublist(list1, list2) ? SublistType.Sublist : SublistType.Unequal,
            (int a, int b) when a > b => IsSublist(list2, list1) ? SublistType.Superlist : SublistType.Unequal,
            _ => list1.SequenceEqual(list2) ? SublistType.Equal : SublistType.Unequal,
        };
    static bool IsSublist<T>(List<T> list1, List<T> list2) where T : IComparable 
        => list1.Count == 0 ? true : Enumerable.Range(0, list2.Count - list1.Count + 1)
            .Any(n => list2.Skip(n).Take(list1.Count).ToList().SequenceEqual(list1));
}