using System;

public static class CollatzConjecture
{
    public static int Steps(int number)
    {
        if (number <= 0) throw new ArgumentOutOfRangeException();
        int steps = 0;
        while (number != 1)
        {
            steps++;
            number = Collatz(number);
        }
        return steps;
    }
    private static int Collatz(int n)
    {
        return (n % 2) switch
        {
            0 => n / 2,
            1 => 3 * n + 1,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}