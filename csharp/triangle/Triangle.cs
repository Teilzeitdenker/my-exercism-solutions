using System;

public static class Triangle
{
    public static bool IsScalene(double a, double b, double c)
    {
        if (TriangleInequalitiesFulfilled(a, b, c))
        {
            return !IsIsosceles(a, b, c) && !IsEquilateral(a, b, c);
        }
        else return false;
    }

    public static bool IsIsosceles(double a, double b, double c) 
    {
        if (TriangleInequalitiesFulfilled(a, b, c))
        {
            return (a == b) || (b == c) || (a == c);
        }
        else return false;
    }

    public static bool IsEquilateral(double a, double b, double c) 
    {
        if (TriangleInequalitiesFulfilled(a, b, c))
        {
            return (a == b) && (b == c);
        }
        else return false;
    }
    private static bool TriangleInequalitiesFulfilled(double a, double b, double c)
    {

        return (a > 0) && (b > 0) && (c > 0) && (a + b > c) && (a + c > b) && (b + c > a);
    }
}