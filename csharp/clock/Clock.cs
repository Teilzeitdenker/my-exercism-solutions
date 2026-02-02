using System;

public class Clock
{
    private int _minutes;
    public (int, int) Display { get; init; }
    private const int DAY = 1440; 
    public Clock(int hours, int minutes)
    {
        int raw = hours * 60 + minutes;
        // this is the modulo operator defined in terms of the remainder (%) operator
        _minutes = (raw % DAY + DAY) % DAY;
        Display = (_minutes / 60, _minutes % 60);
    }

    public Clock Add(int minutesToAdd)
    {
        return new Clock(Display.Item1, Display.Item2 + minutesToAdd);
    }

    public Clock Subtract(int minutesToSubtract)
    {
        return new Clock(Display.Item1, Display.Item2 - minutesToSubtract);
    }

    public static bool operator ==(Clock c1, Clock c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(Clock c1, Clock c2)
    {
        return !(c1 == c2);
    }

    public override bool Equals(object obj)
    {
        if (obj is Clock c)
        {
            return c.Display == this.Display;
        }
        return false;
    }

    public override int GetHashCode()
    {
        string display = $"%02i{Display.Item1}:%02i{Display.Item2}";
        return String.GetHashCode(display);
    }

    public override string ToString() => $"{Display.Item1:d2}:{Display.Item2:d2}";
}
