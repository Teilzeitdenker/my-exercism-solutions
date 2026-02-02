using System;
using System.Collections.Generic;
using System.Linq;

public class BinTree
{
    public BinTree(int value, BinTree left, BinTree right) => (Value, Left, Right) = (value, left, right);
    public int Value { get; }
    public BinTree Left { get; }
    public BinTree Right { get; }
    public override bool Equals(object obj)
    {
        if (obj is not BinTree b || b.Value != Value) return false;
        bool leftIsSame = (b.Left == null && Left == null) || (Left != null && Left.Equals(b.Left));
        bool rightIsSame = (b.Right == null && Right == null) || (Right != null && Right.Equals(b.Left));
        return leftIsSame && rightIsSame;
    }
    public override int GetHashCode() => base.GetHashCode();
}

public class Zipper
{   
    public Zipper(IEnumerable<(BinTree, bool)> parents, BinTree focus)
    {
        Parents = parents;
        Focus = focus;
    }
    public IEnumerable<(BinTree, bool)> Parents { get; }
    public BinTree Focus { get; }
    public int Value()
    {
        return Focus.Value;
    }

    public Zipper SetValue(int newValue)
    {
        return FromTree(new BinTree(newValue, Focus.Left, Focus.Right), Parents);
    }

    public Zipper SetLeft(BinTree binTree)
    {
        return FromTree(new BinTree(Focus.Value, binTree, Focus.Right), Parents);
    }

    public Zipper SetRight(BinTree binTree) 
    {
        return FromTree(new BinTree(Focus.Value, Focus.Left, binTree), Parents);
    }

    public Zipper Left()
    {
        return Focus.Left != null ? FromTree(Focus.Left, Parents.Prepend((Focus, true))) : null;
    }

    public Zipper Right()
    {
        return Focus.Right != null ? FromTree(Focus.Right, Parents.Prepend((Focus, false))) : null;
    }

    public Zipper Up()
    {
        if (!Parents.Any()) return null;
        var (tree, isLeft) = Parents.First();
        var rest = Parents.Skip(1);
        var newFocus = new BinTree(tree.Value, isLeft ? Focus : tree.Left, isLeft ? tree.Right : Focus);
        return FromTree(newFocus, rest);
    }

    public BinTree ToTree()
    {
        if (Parents.Count() == 0) return Focus;
        Zipper actual = this;
        while (actual.Parents.Count() > 0)
        {
            actual = actual.Up();
        }
        return actual.Focus;
    }

    public static Zipper FromTree(BinTree tree, IEnumerable<(BinTree, bool)> parents = null)
    {
        return new Zipper(parents ?? Enumerable.Empty<(BinTree, bool)>(), tree);
    }
    public override bool Equals(object obj) => (obj as Zipper).Focus.Equals(this.Focus);
    public override int GetHashCode() => base.GetHashCode();
}