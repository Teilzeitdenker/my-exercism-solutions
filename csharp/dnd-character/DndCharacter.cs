using System;
using System.Linq;

public class DndCharacter
{
    public DndCharacter(int s, int d, int c, int i, int w, int ch, int h)
    {
        Strength = s;
        Dexterity = d;
        Constitution = c;
        Intelligence = i;
        Wisdom = w;
        Charisma = ch;
        Hitpoints = h;
    }
    public int Strength { get; }
    public int Dexterity { get; }
    public int Constitution { get; }
    public int Intelligence { get; }
    public int Wisdom { get; }
    public int Charisma { get; }
    public int Hitpoints { get; }
    public static int Modifier(int score)
    {
        if (score >= 10)
            return (score - 10) / 2;
        else return -((11 - score) / 2 );
    }

    public static int Ability() 
    {
        Random r = new Random();
        int[] dices = new int[] { r.Next(1, 7), r.Next(1, 7), r.Next(1, 7), r.Next(1, 7)};
        return dices.Sum() - dices.Min();
    }

    public static DndCharacter Generate()
    {
        int c = Ability();
        return new DndCharacter(Ability(), Ability(), c, Ability(), Ability(), Ability(), 10 + Modifier(c));
    }
}
