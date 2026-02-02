using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Node : IEnumerable<Attr>, IComparable
{
    public string Name;
    public IList<Attr> Attributes = new List<Attr>(); 
    public Node(string name) => Name = name;
    public void Add(string key, string value) => Attributes.Add(new Attr(key, value));
    public IEnumerator<Attr> GetEnumerator() => Attributes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public override bool Equals(object obj) => 
        this.Name == (obj as Node).Name 
        && this.Attributes.SequenceEqual((obj as Node).Attributes);
    public override int GetHashCode() => Name.GetHashCode();
    public int CompareTo(object obj) => this.Name.CompareTo((obj as Node).Name);
}

public class Edge : IEnumerable<Attr>, IComparable
{
    public string Left;
    public string Right;
    public IList<Attr> Attributes = new List<Attr>();
    public Edge(string left, string right) => (Left, Right) = (left, right);
    public void Add(string key, string value) => Attributes.Add(new Attr(key, value));
    public IEnumerator<Attr> GetEnumerator() => Attributes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public override bool Equals(object obj) => 
        (this.Left, this.Right) == ((obj as Edge).Left, (obj as Edge).Right) 
        && this.Attributes.SequenceEqual((obj as Edge).Attributes);
    public override int GetHashCode() => Left.GetHashCode() ^ Right.GetHashCode();
    public int CompareTo(object obj) => this.Left.CompareTo((obj as Edge).Left);
}

public class Attr : IComparable
{
    public string Key;
    public string Value;
    public Attr(string key, string value) => (Key, Value) = (key, value);
    public override bool Equals(object obj) => this.Key == (obj as Attr).Key && this.Value == (obj as Attr).Value;
    public override int GetHashCode() => Key.GetHashCode() ^ Value.GetHashCode();
    public int CompareTo(object obj) => this.Key.CompareTo((obj as Attr).Key);
}

public class Graph : IEnumerable<Attr>
{
    public List<Attr> Attrs { get; } = new List<Attr>();
    public List<Node> Nodes { get; } = new List<Node>();
    public List<Edge> Edges { get; } = new List<Edge>();
    public void Add(string key, string value)
    {
        Attrs.Add(new Attr(key, value));
        Attrs.Sort();
    }
    public void Add(Node node)
    {
        Nodes.Add(node);
        Nodes.Sort();
    }
    public void Add(Edge edge)
    {
        Edges.Add(edge);
        Edges.Sort();
    }
    public IEnumerator<Attr> GetEnumerator() => Attrs.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}