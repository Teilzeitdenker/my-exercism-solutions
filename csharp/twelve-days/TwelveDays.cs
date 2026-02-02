using System;
using System.Collections.Generic;
using System.Linq;

public static class TwelveDays
{
    private static Dictionary<int, string> gifts = new Dictionary<int, string>()
    {
        { 1, "a Partridge in a Pear Tree." },
        { 2, "two Turtle Doves, and " },
        { 3, "three French Hens, " },
        { 4, "four Calling Birds, " },
        { 5, "five Gold Rings, " },
        { 6, "six Geese-a-Laying, " },
        { 7, "seven Swans-a-Swimming, " },
        { 8, "eight Maids-a-Milking, " },
        { 9, "nine Ladies Dancing, " },
        { 10, "ten Lords-a-Leaping, " },
        { 11, "eleven Pipers Piping, " },
        { 12, "twelve Drummers Drumming, " }
    };

    private static string[] ordinals = new string[]
    {
        null, "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth", "tenth", "eleventh", "twelfth"
    };

    public static string Recite(int verseNumber)
    {
        if (verseNumber < 1 || verseNumber > 12) throw new ArgumentException("verseNumber out of range [1..12]");
        string allGifts = string.Join("", Enumerable.Range(1, verseNumber).Reverse().Select(i => gifts[i]));
        return $"On the {ordinals[verseNumber]} day of Christmas my true love gave to me: " + allGifts;
    }

    public static string Recite(int startVerse, int endVerse) =>
        string.Join('\n', Enumerable.Range(startVerse, endVerse - startVerse + 1).Select(i => Recite(i)));
}