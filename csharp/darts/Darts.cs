using System;

public static class Darts
{
    public static int Score(double x, double y)
    {
        double r = Math.Sqrt(x * x + y * y);
        if (r > 10.0) return 0;
        if (r <= 1.0) return 10;
        if (r <= 5.0) return 5;
        else return 1;
    }
}
