using System;

public static class DifferenceOfSquares
{
    public static int CalculateSquareOfSum(int max)
    {
        int sumToN = (max * (max + 1)) / 2;
        return sumToN * sumToN;
    }

    public static int CalculateSumOfSquares(int max)
    {
        return (max * (max + 1) * (2 * max + 1)) / 6;
    }

    public static int CalculateDifferenceOfSquares(int max)
    {
        return CalculateSquareOfSum(max) - CalculateSumOfSquares(max);
    }
}