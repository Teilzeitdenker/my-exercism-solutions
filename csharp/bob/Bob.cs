using System;
using System.Linq;

public static class Bob
{
    private static bool IsQuestion(string s) => s.Length == 0 ? false : s.Last() == '?';
    private static bool IsYelled(string s) => ContainsLetters(s) ? s.ToUpper() == s : false;
    private static bool ContainsLetters(string s) => s.Any(c => Char.IsLetter(c));
    public static string Response(string statement)
    {
        string s = statement.Trim();
        if (s == "") return "Fine. Be that way!";
        else return (IsQuestion(s), IsYelled(s)) switch
        {
            (true, true) => "Calm down, I know what I'm doing!",
            (true,   _ ) => "Sure.",
            (  _ , true) => "Whoa, chill out!",
            (  _ ,   _ ) => "Whatever."
        };
    }
}