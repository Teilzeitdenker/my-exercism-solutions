using System;
using System.Collections.Generic;
using System.Linq;

public static class AccumulateExtensions
{
    public static IEnumerable<U> Accumulate<T, U>(this IEnumerable<T> collection, Func<T, U> func)
    {
        // This isn't lazy
        //List<U> result = new List<U>();
        //foreach (var item in collection)
        //{
        //    result.Add(func.Invoke(item));
        //}
        //return result;
        return collection.Select(func);
    }
}