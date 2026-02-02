using System;

public class SpaceAge
{
    private const double SECONDS_PER_YEAR = 31557600.0;
    public double Seconds { get; init; }
    public SpaceAge(int seconds)
    {
        Seconds = seconds;
    }

    public double OnEarth()
    {
        return System.Math.Round(Seconds / (SECONDS_PER_YEAR * 1.0), 2);
    }

    public double OnMercury()
    {
        return System.Math.Round(Seconds / (SECONDS_PER_YEAR * 0.2408467), 2);
    }

    public double OnVenus()
    {
        return System.Math.Round(Seconds / (SECONDS_PER_YEAR * 0.61519726), 2);
    }

    public double OnMars()
    {
        return System.Math.Round( Seconds / (SECONDS_PER_YEAR * 1.8808158), 2);
    }

    public double OnJupiter()
    {
        return System.Math.Round( Seconds / (SECONDS_PER_YEAR * 11.862615), 2);
    }

    public double OnSaturn()
    {
        return System.Math.Round( Seconds / (SECONDS_PER_YEAR * 29.447498), 2);
    }

    public double OnUranus()
    {
        return System.Math.Round( Seconds / (SECONDS_PER_YEAR * 84.016846), 2);
    }

    public double OnNeptune()
    {
        return System.Math.Round( Seconds / (SECONDS_PER_YEAR * 164.79132), 2);
    }
}