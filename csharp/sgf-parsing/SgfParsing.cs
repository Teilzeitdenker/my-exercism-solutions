using System;
using System.Linq;
using System.Collections.Generic;
using Sprache;

public class SgfTree
{
    public SgfTree(IDictionary<string, string[]> data, params SgfTree[] children)
     => (Data, Children) = (data, children);
    public IDictionary<string, string[]> Data { get; }
    public SgfTree[] Children { get; }
}

public class SgfParser
{
    public static SgfTree ParseTree(string input) {
        var parsing = Grammar.Tree.TryParse(input);
        return parsing.WasSuccessful ? parsing.Value : throw new ArgumentException();
    }
}
public static class Grammar
{
    public static readonly Parser<string> PropertyValue =
        from open in Parse.Char('[').Once()
        from txt in Parse.CharExcept(']').AtLeastOnce().Text()
        from closed in Parse.Char(']').Once()
        select txt;
    public static readonly Parser<KeyValuePair<string, string[]>> Property =
        from propertyKey in Parse.Letter.Many().Text()
        from propertyValues in PropertyValue.Many()
        let key = propertyKey.Any(ch => char.IsLower(ch)) ?
            throw new ArgumentException() : propertyKey
        let values = propertyKey.Length > 0 && propertyValues.Count() == 0 ?
            throw new ArgumentException() : propertyValues.ToArray()
        select new KeyValuePair<string, string[]>(key, values);
    public static readonly Parser<SgfTree> Child =
        from open in Parse.Optional(Parse.Char('('))
        from semicolon in Parse.Char(';')
        from properties in Property.Many()
        from closed in Parse.Optional(Parse.Char(')'))
        select new SgfTree(properties.ToDictionary());
    public static readonly Parser<SgfTree> Tree =
        from open in Parse.Char('(').Once()
        from semicolon in Parse.Char(';')
        from data in Property.Many()
        from children in Child.Many()
        select new SgfTree(data.ToDictionary(), children.ToArray());
}