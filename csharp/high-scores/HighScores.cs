using System;
using System.Collections.Generic;
using System.Linq;

public class HighScores
{
    private List<int> scores = new List<int>();
    private List<int> sorted;
    public HighScores(List<int> list)
    {
        // copy the unsorted list
        foreach (int i in list) scores.Add(i);
        list.Sort();
        list.Reverse();
        sorted = list;
    }

    public List<int> Scores()
    {
        return scores;
    }

    public int Latest()
    {
        return scores.Last();
    }

    public int PersonalBest()
    {
        return sorted.First();
    }

    public List<int> PersonalTopThree()
    {
        if (sorted.Count >= 3)
            return sorted.GetRange(0, 3).ToList<int>();
        else return sorted;
    }
}