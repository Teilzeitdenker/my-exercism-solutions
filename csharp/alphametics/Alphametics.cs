using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using Combinatorics.Collections;

public static class Alphametics
{
    public static IDictionary<char, int> Solve(string equation) =>   
        Grammar.Problem.Parse(equation).Solve();
}

public static class Grammar
{
    // using the Sprache library (v2.3.1) to parse the Problem input
    private static readonly Parser<String> Word = Parse.Upper.Many().Text().Token();
    private static readonly Parser<Char> Plus = Parse.Char('+');
    private static readonly Parser<String> Equal = Parse.String("==").Text();
    public static readonly Parser<Problem> Problem =
        from summands in Word.DelimitedBy(Plus)
        from equalSign in Equal
        from result in Word.End()
        select new Problem(summands, result);
}

public class Problem
{
    public Problem(IEnumerable<string> summands, string result)
    {
        var charValues = new Dictionary<char, int>();
        var allChars = new HashSet<char>();
        nonZeroSet = new HashSet<char>();
        foreach (var summand in summands) { // process summands
            nonZeroSet.Add(summand[0]);
            int idx = 0;
            foreach (var ch in summand.Reverse())
            {
                allChars.Add(ch);
                int to_add = (int)Math.Pow(10, idx++);
                if (charValues.ContainsKey(ch))
                    charValues[ch] += to_add;
                else charValues[ch] = to_add;
            }
        }
        { // process result
            nonZeroSet.Add(result[0]);
            int idx = 0;
            foreach (var ch in result.Reverse())
            {
                allChars.Add(ch);
                int to_subtract = (int)Math.Pow(10, idx++);
                if (charValues.ContainsKey(ch))
                    charValues[ch] -= to_subtract;
                else charValues[ch] = -to_subtract;
            }
        }
        // split charValues into two lists to fix an order when assigning
        // a permutation of digits and to have better access
        chars = allChars.ToList();
        values = chars.Select(ch => charValues[ch]).ToList();

    } 
    private readonly IEnumerable<int> Digits = Enumerable.Range(0, 10);
    private readonly HashSet<char> nonZeroSet;
    private readonly List<char> chars;
    private readonly List<int> values;
    public IDictionary<char, int> Solve()
    {      
        // using the Combinatorics library (v2.0.0) for combinations and permutations
        Combinations<int> combinations = new Combinations<int>(Digits, chars.Count);
        foreach (var comb in combinations)
        {
            Permutations<int> permutations = new Permutations<int>(comb);
            foreach (var perm in permutations)
            {
                // calculate the product of the assigned digits with the place values of the letters
                var calc = values.Zip(perm).Aggregate(0, (acc, t) => acc + t.First * t.Second);
                if (calc != 0) continue;
                // have to check that 0 isn't assigned to a letter that must be nonzero
                var tmp = perm.Zip(chars).ToDictionary();
                if (tmp.Keys.Contains(0) && nonZeroSet.Contains(tmp[0])) continue;
                // otherwise we found a solution
                return chars.Zip(perm).ToDictionary();
            }
        }
        throw new ArgumentException("no solution");
    }
}