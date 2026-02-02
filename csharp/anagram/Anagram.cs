using System;
using System.Linq;
public class Anagram
{
    public readonly string _target;
    public readonly string _sortedTarget;
    public Anagram(string baseWord) { _target = baseWord.ToLower(); _sortedTarget = SortedLower(_target); }
    public string[] FindAnagrams(string[] potentialMatches) => 
        potentialMatches.Where(w => !w.ToLower().Equals(_target) && SortedLower(w).Equals(_sortedTarget) ).ToArray();
    public string SortedLower(string word) =>  new string(word.ToLower().OrderBy(c => c).ToArray());
}

