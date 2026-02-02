using System;

public static class ResistorColorDuo
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
        return 10 * ColorCode(colors[0]) + ColorCode(colors[1]);
    }
}
