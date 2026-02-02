using System;
using System.Collections.Generic;

public class Robot
{
    private static readonly Random rnd = new Random();
    // although "readonly" items can be added
    // must be static in order to be seen by ALL robots
    private static readonly HashSet<string> given = new HashSet<string>();
    public string Name { get; private set; }
    public Robot()
    {
        Reset();
    }
    public void Reset()
    {
        bool done = false;
        string name = String.Empty;
        while (!done)
        {
            char firstLetter = (char)rnd.Next(65, 91);
            char secondLetter = (char)rnd.Next(65, 91);
            char firstDigit = (char)rnd.Next(48, 58);
            char secondDigit = (char)rnd.Next(48, 58);
            char thirdDigit = (char)rnd.Next(48, 58);
            name = new string(new char[] { firstLetter, secondLetter, firstDigit, secondDigit, thirdDigit });
            if (given.Add(name))
            {
                done = true;
            }
        }
        Name = name;
    }
}