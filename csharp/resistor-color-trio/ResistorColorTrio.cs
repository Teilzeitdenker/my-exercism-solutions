using System;

public static class ResistorColorTrio
{
    private enum ColorCodes
    {
        black,
        brown,
        red,
        orange,
        yellow,
        green,
        blue,
        violet,
        grey,
        white,
    }
    public static int ColorCode(string color)
    {
        // can even make it case insensitive with true as second argument.
        return (int)Enum.Parse<ColorCodes>(color, true);
    }
    public static int Value(string[] colors)
    {
        int val = 10 * ColorCode(colors[0]) + ColorCode(colors[1]);
        for (int i = 0; i < ColorCode(colors[2]); ++i) val *= 10;
        return val;
    }
    public static string Label(string[] colors)
    {
        int val = Value(colors);
        bool divisibleByThousand = val % 1000 == 0;
        return divisibleByThousand ? (val / 1000).ToString() + " kiloohms" : val.ToString() + " ohms"; 
    }
}
