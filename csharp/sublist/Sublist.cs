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
    public static SublistType Classify<T>(List<T> list1, List<T> list2)
        where T : IComparable => (list1.Count, list2.Count) switch
        {
            (0, 0) => SublistType.Equal,
            (0, _) => SublistType.Sublist,
            (_, 0) => SublistType.Superlist,
            (int a, int b) when a < b => IsSublist(list1, list2) ? SublistType.Sublist : SublistType.Unequal,
            (int a, int b) when a > b => IsSublist(list2, list1) ? SublistType.Superlist : SublistType.Unequal,
            _ => IsSameList(list1, list2) ? SublistType.Equal : SublistType.Unequal,
        };

    static bool IsSublist<T>(List<T> list1, List<T> list2) 
        where T : IComparable => Enumerable.Range(0, list2.Count - list1.Count + 1)
            .Select(n => list2.Skip(n).Take(list1.Count).ToList()).Any(ls => IsSameList(ls, list1));

    static bool IsSameList<T>(List<T> list1, List<T> list2)
        where T : IComparable
    {
        if (list1.Count != list2.Count) { return false; }
        for (int i = 0; i < list1.Count; i++)
        {
            if (!list1[i].Equals(list2[i])) { return false; }
        }
        return true;
    }
}