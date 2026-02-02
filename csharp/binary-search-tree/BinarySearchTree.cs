using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BinarySearchTree : IEnumerable<int>
{
    public BinarySearchTree(int value) => Value = value;
    public BinarySearchTree(IEnumerable<int> values)
    {
        if (values == null || !values.Any())
        {
            throw new ArgumentException($"{nameof(values)} is null or empty!");
        }
        Value = values.First();
        foreach (int val in values.Skip(1))  Add(val);
    }
    public int Value { get; private set; }
    public BinarySearchTree Left { get; private set; }
    public BinarySearchTree Right { get; private set; }
    public BinarySearchTree Add(int value)
    {
        if (value <= Value && Left == null)
        {
            Left = new BinarySearchTree(value);
        }
        else if (value <= Value)
        {
            Left.Add(value);
        }
        else if (Right == null)
        {
            Right = new BinarySearchTree(value);
        }
        else
        {
            Right.Add(value);
        }
        return this;
    }
    public IEnumerator<int> GetEnumerator()
    {
        if (Left != null)
            foreach (int val in Left)
                yield return val;
        
        yield return Value;
        
        if (Right != null)
            foreach (int val in Right)
                yield return val;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}