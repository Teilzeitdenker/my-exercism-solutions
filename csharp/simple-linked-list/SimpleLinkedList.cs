using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SimpleLinkedList<T> : IEnumerable<T>
{
    // Node class
    private class Node
    {
        public T Data { get; init; }
        public Node Next { get; set; }  
    }
    // Properties
    private Node head = null;
    public int Count { get; private set; } = 0;
    // Constructors
    public SimpleLinkedList() { }
    public SimpleLinkedList(T value) => Push(value);
    public SimpleLinkedList(IEnumerable<T> values)
    {
        foreach (T value in values) { Push(value); }
    }
    // Methods
    public void Push(T value)
    {
        Node node = new Node { Data = value };
        node.Next = head;
        head = node;
        Count++;
    }
    public T Pop()
    {
        if (Count == 0) { throw new Exception("empty list");  }
        T ret = head.Data;
        head = head.Next;
        Count--;
        return ret;
    }
    public void Reverse()
    {
        T[] values = new T[Count];
        for (int i = 1; i <= Count; i++) { values[Count - i] = Pop(); }
        foreach (T value in values) { Push(value); }
    }
    // Interface implementation
    public IEnumerator<T> GetEnumerator()
    {
        Node hd = head;
        while (hd != null)
        {
            yield return hd.Data;
            hd = hd.Next;
        }
    } 
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}