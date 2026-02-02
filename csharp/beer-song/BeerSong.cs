using System;
using System.Text;

public static class BeerSong
{
    public static string Recite(int startBottles, int takeDown)
    {
        int count = 0;
        StringBuilder b = new StringBuilder();
        for (int i = startBottles; i >= 0; i--)
        {
            if (count == takeDown) return b.ToString();
            if (i < startBottles) b.Append("\n\n");
            if (i > 2)
            {
                b.Append($"{i} bottles of beer on the wall, {i} bottles of beer.\n");
                b.Append($"Take one down and pass it around, {i - 1} bottles of beer on the wall.");
            }
            else if (i == 2)
            {
                b.Append($"{i} bottles of beer on the wall, {i} bottles of beer.\n");
                b.Append($"Take one down and pass it around, {i - 1} bottle of beer on the wall.");
            }
            else if (i == 1) b.Append("1 bottle of beer on the wall, 1 bottle of beer.\nTake it down and pass it around, no more bottles of beer on the wall.");
            else if (i == 0) b.Append("No more bottles of beer on the wall, no more bottles of beer.\nGo to the store and buy some more, 99 bottles of beer on the wall.");
            count += 1;
        }
        return b.ToString();
    }
}