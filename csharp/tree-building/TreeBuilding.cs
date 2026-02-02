using System;
using System.Collections.Generic;
using System.Linq;

public class TreeBuildingRecord
{
    public int ParentId { get; set; }
    public int RecordId { get; set; }
}

public class Tree
{
    public int Id { get; set; }
    public int ParentId { get; set; }

    public List<Tree> Children { get; set; }

    public bool IsLeaf => Children.Count == 0;
}

public static class TreeBuilder
{
    public static Tree BuildTree(IEnumerable<TreeBuildingRecord> records)
    {
        if (records.Count() == 0) throw new ArgumentException("empty records");     
        
        var ordered = new SortedList<int, TreeBuildingRecord>();
        foreach (var record in records)
        {
            ordered.Add(record.RecordId, record);
        }
        if (ordered.Keys.First() != 0)                throw new ArgumentException("missing root");
        if (ordered.Keys.Last() != ordered.Count - 1) throw new ArgumentException("non-continuous list");
        
        records = ordered.Values;
        var trees = new List<Tree>();

        foreach (var record in records)
        {
            if (record.RecordId == 0 && record.ParentId != 0) throw new ArgumentException("invalid root");
            if (record.RecordId != 0 && record.ParentId >= record.RecordId) throw new ArgumentException("non-root with invalid parent");

            Tree t = new Tree { Id = record.RecordId, ParentId = record.ParentId, Children = new List<Tree>() };
            if (record.RecordId != 0)
            {
                var parent = trees.First(tree => tree.Id == record.ParentId);
                parent.Children.Add(t);
            }
            trees.Add(t);
        }
        return trees.First(t => t.Id == 0);
    }
}