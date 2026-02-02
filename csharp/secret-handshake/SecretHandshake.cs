using System;
using System.Linq;
using System.Collections.Generic;

public static class SecretHandshake
{
    public static string[] Commands(int commandValue)
    {
        if (commandValue <= 0 || commandValue >= 32) return new string[0];
        return RightOrder(commandValue, commandValue >= 16);
    }
    private static string[] RightOrder(int without16, bool biggerThan16)
    {
        IEnumerable<(int, string)> facsOf2 = new[] { (1, "wink"), (2, "double blink"), (4, "close your eyes"), (8, "jump") };
        if (biggerThan16)
            facsOf2 = facsOf2.Reverse();
        return facsOf2.Where(v => (v.Item1 | without16) == without16).Select(v => v.Item2).ToArray();
    }
}
