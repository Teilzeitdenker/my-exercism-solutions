using System;
using System.Collections.Generic;
using System.Linq;

public class RailFenceCipher
{
    public int Rails { get; init; }
    public RailFenceCipher(int rails) => Rails = rails;
    public string Encode(string input)
    {
        return String.Join("", UpAndDown()
            .Zip(input)
            .GroupBy(t => t.First, t => t.Second)
            .Select(c => string.Join("", c)));
    }
    public string Decode(string input)
    {
        return string.Join("", UpAndDown()
            .Zip(Enumerable.Range(0, input.Length))
            .OrderBy(t => t.First)
            .Select(t => t.Second)
            .Zip(input)
            .OrderBy(t => t.First)
            .Select(t => t.Second));
    }
    public IEnumerable<int> UpAndDown()
    {
        while (true)
        {
            for (int i = 1; i < Rails; i++)
            {
                yield return i;
            }
            for (int i = Rails; i > 1; i--)
            {
                yield return i;
            }
        }
    } 
}