using System;
using System.Collections.Generic;
using System.Linq;

public class WordSearch
{

    private Dictionary<char, List<(int, int)>> gridDict;
    private readonly (int, int)[] directions = new[] { (1,0),(1,1),(0,1),(-1,1),(-1,0),(-1,-1),(0,-1),(1,-1) };
    public WordSearch(string grid)
    {
        gridDict = new Dictionary<char, List<(int, int)>>();
        string[] gridRows = grid.Split('\n', StringSplitOptions.TrimEntries).ToArray();
        for (int j = 0; j < gridRows.Length; j++)
        {
            for (int i = 0; i < gridRows[j].Length; i++)
            {
                (int, int) actualPos = (i + 1, j + 1);
                char actualChar = gridRows[j][i];
                if (gridDict.ContainsKey(actualChar))
                {
                    gridDict[actualChar].Add(actualPos);
                } 
                else
                {
                    gridDict.Add(actualChar, new List<(int, int)> { actualPos });
                }
            }
        }
    }

    public Dictionary<string, ((int, int), (int, int))?> Search(string[] wordsToSearchFor) =>
        wordsToSearchFor.Select(word => CheckWord(word)).ToDictionary(el => el.Item1, el => el.Item2);

    private (string, ((int, int),(int, int))?) CheckWord(string word)
    {
        if (word.Any(c => !gridDict.ContainsKey(c))) { return (word, null); }
        
        IEnumerable<((int, int), (int, int))> results = gridDict[word[0]]
            .SelectMany(start => directions.Select(dir => (start, dir)))
            .Where(comb => CheckDirection(word, comb.start, comb.dir));
        
        if (results.Any())
        {
            (int, int) start = results.First().Item1;
            (int, int) dir = results.First().Item2;
            (int, int) endPos = addTuples(start, scalarMult(word.Length - 1, dir));
            return (word, (start, endPos));
        }
        return (word, null);
    }

    private bool CheckDirection(string word, (int, int) start, (int, int) dir)
    {
        for (int i = 1; i < word.Length; i++)
        {
            (int, int) pos = addTuples(start, scalarMult(i, dir));
            if (!gridDict[word[i]].Contains(pos)) { return false; }
        }
        return true;
    }

    private (int, int) addTuples((int, int) t1, (int, int) t2) => (t1.Item1 + t2.Item1, t1.Item2 + t2.Item2);
    private (int, int) scalarMult(int k, (int, int) t) => (k * t.Item1, k * t.Item2);
}