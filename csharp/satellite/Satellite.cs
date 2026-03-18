using System;
using System.Linq;

public record Tree(char Value, Tree? Left, Tree? Right);

public static class Satellite
{
    public static Tree? TreeFromTraversals(char[] preOrder, char[] inOrder)
    {
        if (preOrder.Length != inOrder.Length)
            throw new ArgumentException("traversals must have the same length");
        
        if (!preOrder.OrderBy(x => x).SequenceEqual(inOrder.OrderBy(x => x)))
            throw new ArgumentException("traversals must have the same elements");
        
        if (preOrder.Length != preOrder.Distinct().Count() || inOrder.Length != inOrder.Distinct().Count())
            throw new ArgumentException("traversals must contain unique items");

        return BuildTree(preOrder, inOrder);
    }

    private static Tree? BuildTree(char[] preOrder, char[] inOrder)
    {
        if (preOrder.Length == 0) return null;
        if (preOrder.Length == 1) return new Tree(preOrder[0], null, null);
        
        char root = preOrder[0];
        int rootIndex = Array.IndexOf(inOrder, root);

        var leftInOrder = inOrder.Take(rootIndex).ToArray();
        var rightInOrder = inOrder.Skip(rootIndex + 1).ToArray();

        var leftPreOrder = preOrder.Skip(1).Take(leftInOrder.Length).ToArray();
        var rightPreOrder = preOrder.Skip(1 + leftInOrder.Length).ToArray();

        return new Tree(root, BuildTree(leftPreOrder, leftInOrder), BuildTree(rightPreOrder, rightInOrder));
    }
}
