using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SimpleLinkedList<T> : IEnumerable<T>
{
    //Properties
    private List<T> list = new List<T>();
    public int Count
    {
        get
        {
            return list.Count();
        }
        private set { }
    }
    // Constructors
    public SimpleLinkedList() { }
    public SimpleLinkedList(T value)
    {
        Push(value);
    }
    public SimpleLinkedList(IEnumerable<T> values)
    {
        foreach (T value in values) { Push(value); }
    }
    // Methods
    public void Push(T value)
    {
        list.Add(value);
    }
    public T Pop()
    {
        if (list.Count == 0) { throw new Exception("empty list");  }
        T last = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return last;
    }
    public void Reverse()
    {
        list.Reverse();
    }
    // Interface implementation
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            yield return list[i];
        }
    } 
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}