using System.Linq;
using System.Collections.Generic;

public static class Transpose
{
    public static string String(string input)
    {
        if (input == "") return "";
        var len = input.Split('\n').Select(word => word.Length).Max();
        var charMatrix = input.Split('\n').Select(word => word.PadRight(len, '*')).ToArray();
        var transpose = new List<System.String>();
        for (var i = 0; i < len; i++)
        {
            var line = new System.String("");
            for (var j = 0; j < charMatrix.Length; j++)
            {
                line += charMatrix[j][i];
            }
            transpose.Add(line.TrimEnd('*').Replace('*', ' '));
        }
        return System.String.Join("\n", transpose);
    }
}