using System;
using System.Collections.Generic;

public class Robot
{
    HashSet<string> given = new HashSet<string>();
    public string Name { get; set; }
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
            Random rnd = new Random();
            char firstLetter = (char)rnd.Next(65, 91);
            char secondLetter = (char)rnd.Next(65, 91);
            char firstDigit = (char)rnd.Next(48, 58);
            char secondDigit = (char)rnd.Next(48, 58);
            char thirdDigit = (char)rnd.Next(48, 58);
            name = new string(new char[] { firstLetter, secondLetter, firstDigit, secondDigit, thirdDigit });
            if (!given.Contains(name))
            {
                done = true;
                given.Add(name);
            }
        }
        Name = name;
    }
}