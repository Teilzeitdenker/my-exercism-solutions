using System.Collections.Generic;
using System.Linq;

public static class Diamond
{
    // Small extension method for IEnumerable<T> (@copyright petertseng on Exercism)
    // Concats any enumerable with its reverse without repeating the last element (which is the reflection axis)
    internal static IEnumerable<T> Reflect<T>(this IEnumerable<T> ls) => ls.Concat(ls.Reverse().Skip(1));
    public static string Make(char target)
    {
        var n = target - 'A'; // total white space in half of every row 
        string getRow(char letter)
        {
            var x = letter - 'A'; // whitespace counted from the middle of the row
            var leftPart = "".PadRight(n - x) + letter + "".PadRight(x);
            return string.Concat(leftPart.Reflect()); // here the generic parameter T will be char
        }
        var upperPart = Enumerable.Range('A', target - 'A' + 1).Select(c => getRow((char)c));
        return string.Join('\n', upperPart.Reflect()); // here the generic parameter T will be string
    }
}