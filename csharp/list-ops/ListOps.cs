using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

public static class ListOps
{
    public static int Length<T>(List<T> input) => input.Count;
    public static List<T> Reverse<T>(List<T> input)
    {
        input.Reverse();
        return input;
    }
    public static List<TOut> Map<TIn, TOut>(List<TIn> input, Func<TIn, TOut> map) => input.Select(map).ToList();
    public static List<T> Filter<T>(List<T> input, Func<T, bool> predicate) => input.Where(predicate).ToList();
    public static TOut Foldl<TIn, TOut>(List<TIn> input, TOut start, Func<TOut, TIn, TOut> func) => input.Aggregate(start, func);
    public static TOut Foldr<TIn, TOut>(List<TIn> input, TOut start, Func<TIn, TOut, TOut> func)
    {
        input.Reverse();
        return input.Aggregate(start, (acc, item) => func(item, acc));
    }
    public static List<T> Concat<T>(List<List<T>> input) => input.SelectMany(c => c).ToList();
    public static List<T> Append<T>(List<T> left, List<T> right)
    {
        left.AddRange(right);
        return left;
    }
}