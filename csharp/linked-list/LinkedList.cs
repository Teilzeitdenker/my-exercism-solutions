using System;
using System.Collections.Generic;

public class Deque<T>
{
    private class Node
    {
        public T Data { get; init; }
        public Node Prev { get; set; }
        public Node Next { get; set; }
    }
    private int _count = 0;
    private Node _head = null;
    private Node _tail = null;

    public Deque() { }
    public Deque(IEnumerable<T> values)
    {
        foreach (T value in values) { Push(value); }
    }
    public void Push(T value)
    {
        Node e = new Node { Data = value };
        if (_count == 0)
        {
            _head = e;
            _tail = e;
        }
        else
        {
            _tail.Next = e;
            e.Prev = _tail;
            _tail = e;
        }
        _count++;
    }

    public T Pop()
    {
        if (_count == 0) { throw new Exception("empty list"); }
        T ret = _tail.Data;
        if (_tail.Prev != null)
        {
            _tail = _tail.Prev;
            _tail.Next = null;
        } else
        {
            _head = null;
            _tail = null;
        }
        _count--;
        return ret;
    }

    public void Unshift(T value)
    {
        Node e = new Node { Data = value };
        if (_count == 0)
        {
            _head = e;
            _tail = e;
        }
        else
        {
            e.Next = _head;
            _head.Prev = e;
            _head = e;
        }
        _count++;
    }

    public T Shift()
    {
        if (_count == 0) { throw new Exception("empty list"); }
        T ret = _head.Data;
        if (_head.Next != null)
        {
            _head = _head.Next;
            _head.Prev = null;
        } else
        {
            _head = null;
            _tail = null;
        }
        _count--;
        return ret;
    }
    // named iterators
    public IEnumerable<T> GetInOrder()
    {
        Node actual = _head;
        while (actual != null)
        {
            yield return actual.Data;
            actual = actual.Next;
        }
    }
    public IEnumerable<T> GetReverseOrder()
    {
        Node actual = _tail;
        while (actual != null)
        {
            yield return actual.Data;
            actual = actual.Prev;
        }
    }
}