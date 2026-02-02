using System;
using System.Collections.Generic;

public class CircularBuffer<T>
{
    private int _capacity;
    private T[] _buffer;
    private int _reader = 0;
    private int _writer = 0;
    public CircularBuffer(int capacity)
    {
        _capacity = capacity;
        _buffer = new T[_capacity]; 
    }

    public T Read()
    {
        if (_reader >= _writer) throw new InvalidOperationException();
        int indexToRead = _reader % _capacity;
        _reader++;
        return _buffer[indexToRead];
    }

    public void Write(T value)
    {
        if (_writer - _reader == _capacity) throw new InvalidOperationException();
        int indexToWrite = _writer % _capacity;
        _writer++;
        _buffer[indexToWrite] = value;
        return;
    }

    public void Overwrite(T value)
    {
        if (_writer - _reader < _capacity)
        {
            Write(value);
            return;
        }
        int indexToWrite = _reader % _capacity;
        _reader++;
        _writer++;
        _buffer[indexToWrite] = value;
        return;
    }

    public void Clear()
    {
        _reader = 0;
        _writer = 0;
        _buffer.Initialize();
        return;
    }
}