using System;
using System.Collections.Generic;
using System.Linq;

public class Tree
{
    public string Value { get; set; }
    public Tree[] Children { get; set; }
    public Tree(string v, params Tree[] chs)
    {
        Value = v;
        Children = chs;
    }
    public override bool Equals(object obj) => 
        obj is Tree tr && tr.Value == Value && tr.Children.OrderBy(c => c.Value).SequenceEqual(Children.OrderBy(c => c.Value));
    public override int GetHashCode() => Value.GetHashCode();
}

public static class Pov
{
    public static Tree FromPov(Tree tr, string from)
    {
        Tree Reparent(Tree ch, Tree pr)
        {
            var prPersp = new Tree(pr.Value, pr.Children.Where(c => c.Value != ch.Value).ToArray());
            return new Tree(ch.Value, ch.Children.Append(prPersp).ToArray());
        }
        var pth = FindPath(tr, from, Enumerable.Empty<Tree>());
        if (pth != null) { 
            var curr = pth.First();
            foreach (var c in pth.Skip(1)) curr = Reparent(c, curr);
            return curr;
        }
        throw new ArgumentException();
    }
    
    public static IEnumerable<string> PathTo(string from, string to, Tree tr) =>
        FindPath(FromPov(tr, from), to, Enumerable.Empty<Tree>())?.Select(c => c.Value) ?? throw new ArgumentException(); 

#nullable enable
    private static IEnumerable<Tree>? FindPath(Tree curr, string to, IEnumerable<Tree> vis)
    {
        vis = vis.Append(curr);
        if (curr.Value == to) return vis;
        if (curr.Children.Length == 0) return null;
        return curr.Children.Select(c => FindPath(c, to, vis)).FirstOrDefault(v => v != null);
    }
#nullable disable
}