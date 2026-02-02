using System;

public static class Leap
{
    public static bool IsLeapYear(int y)
    {
        // do it the F#-way with pattern matching (instead of nested if clauses)
        return (y % 4, y % 100, y % 400) switch
        {
            (_, _, 0) => true,
            (_, 0, _) => false,
            (0, _, _) => true,
            _ => false
        };
    }
}