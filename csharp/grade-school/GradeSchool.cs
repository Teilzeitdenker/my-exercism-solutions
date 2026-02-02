using System;
using System.Collections.Generic;

public class GradeSchool
{
    private SortedDictionary<int, SortedSet<string>> roster = new SortedDictionary<int, SortedSet<string>>();
    public void Add(string student, int grade)
    {
        if (!roster.ContainsKey(grade)) roster[grade] = new SortedSet<string> { student };
        else roster[grade].Add(student);
    }

    public IEnumerable<string> Roster()
    {
        List<string> sortedNames = new List<string>();
        foreach (int grade in roster.Keys)
        {
            foreach (string name in roster[grade]) sortedNames.Add(name);
        }
        return sortedNames;
    }

    public IEnumerable<string> Grade(int grade)
    {
        if (roster.ContainsKey(grade)) return roster[grade];
        else return new List<string>();
    }
}