using System;

public static class Grains
{
    public static ulong Square(int n)
    {
        if (n <= 0 || n > 64) throw new ArgumentOutOfRangeException();
        return (ulong)Math.Pow(2.0, (double)(n - 1));
    }

    public static ulong Total()
    {
        return 2*Square(64) - 1;
    }
}