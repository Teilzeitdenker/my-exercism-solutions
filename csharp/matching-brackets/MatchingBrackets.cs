using System;
using System.Collections.Generic;

public static class MatchingBrackets
{
    public static bool IsPaired(string input)
    {
        var opened = new List<char>{ '(', '[', '{' };
        var closed = new List<char>{ ')', ']', '}' };
        var brackets = new Stack<char>();
        for (int i = 0; i < input.Length; i++)
        {
            if (!System.Char.IsPunctuation(input[i])) continue;
            if (opened.Contains(input[i])) brackets.Push(input[i]);
            if (closed.Contains(input[i]))
            {
                if (brackets.Count == 0) return false;
                char openBracket = brackets.Pop();
                int openBracketIndex = opened.FindIndex(c => c == openBracket);
                if (openBracketIndex < 0) return false;
                int closedBracketIndex = closed.FindIndex(c => c == input[i]);
                if (openBracketIndex != closedBracketIndex) return false;
            }
        }
        if (brackets.Count != 0)
            return false;
        return true;
    }
}
