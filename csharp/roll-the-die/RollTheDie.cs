using System;

public class Player
{
    public int RollDie()
    {
        var intGen = new System.Random();
        return intGen.Next(1, 18);
    }

    public double GenerateSpellStrength()
    {
        var doubleGen = new System.Random();
        return 100.0 * doubleGen.NextDouble();
    }
}
