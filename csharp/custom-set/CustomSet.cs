using System;
using System.Linq;

public class CustomSet
{
    private int[] _items;
    public CustomSet(params int[] values)
    {
        _items = values.Distinct().ToArray();
    }

    public CustomSet Add(int value)
    {
        return !Contains(value) ? new(_items.Append(value).ToArray()) : new(_items);
    }

    public bool Empty()
    {
        return _items.Length == 0;
    }

    public bool Contains(int value)
    {
        return _items.Contains(value);
    }

    public bool Subset(CustomSet right)
    {
        return _items.All(right.Contains);
    }

    public bool Disjoint(CustomSet right)
    {
        return !_items.Any(right.Contains);
    }

    public CustomSet Intersection(CustomSet right)
    {
        return new(_items.Where(x => right.Contains(x)).ToArray());
    }

    public CustomSet Difference(CustomSet right)
    {
        return new(_items.Where(x => !right.Contains(x)).ToArray());
    }

    public CustomSet Union(CustomSet right)
    {
        return new(_items.Union(right._items).ToArray());
    }

    public override bool Equals(object obj) => obj is CustomSet set && Equals(set);
    public bool Equals(CustomSet right) => _items.All(right.Contains) && right._items.All(Contains);
    public override int GetHashCode() => _items.GetHashCode();
}