using System;
using System.Linq;

public static class Diamond
{
    public static string Make(char target)
    {
        var n = (int)target - (int)'A'; // total white space in half of every row
        string getRow(char letter)
        {
            var x = (int)letter - (int)'A'; // whitespace counted from the middle of the row
            var leftPart = "".PadRight(n - x) + letter.ToString() + "".PadRight(x);
            var rightPart = new string(leftPart.Reverse().Skip(1).ToArray());
            return leftPart + rightPart;
        }
        var upperPart = Enumerable.Range('A', target - 'A' + 1).Select(c => getRow((char)c));
        var lowerPart = upperPart.Reverse().Skip(1);
        return string.Join('\n', upperPart.Concat(lowerPart));
    }
}