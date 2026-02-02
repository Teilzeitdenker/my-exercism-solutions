using System.Text.RegularExpressions;
public static class Markdown
{
    public static string Parse(string input)
    {
        var lvl = Regex.Match(input, "(?m)^(#{1,6})").Groups[1].Length;
        input = Regex.Replace(input, "(?m)^#{1,6} (.+)$", $"<h{lvl}>$1</h{lvl}>");
        input = Regex.Replace(input, "__(.+)__", "<strong>$1</strong>");
        input = Regex.Replace(input, "_(.+)_", "<em>$1</em>");
        input = Regex.Replace(input, @"(?m)^\* (.+)$", "<li>$1</li>");
        input = Regex.Replace(input, "(?s)(<li>.*</li>)", "<ul>$1</ul>");
        input = Regex.Replace(input, "(?m)^(?!<h|<l|<u)(.+)$", "<p>$1</p>");
        return  Regex.Replace(input, "\n", "");
    }
}