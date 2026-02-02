using System;
using System.Linq;
using System.Collections.Generic;

[Flags]
public enum Allergen
{
    Eggs = 1,
    Peanuts = 2,
    Shellfish = 4,
    Strawberries = 8,
    Tomatoes = 16,
    Chocolate = 32,
    Pollen = 64,
    Cats = 128
}

public class Allergies
{
    private Allergen _allergies;
    public Allergies(int mask)
    {
        _allergies = (Allergen)mask;
    }

    public bool IsAllergicTo(Allergen allergen)
    {
        return (_allergies | allergen) == _allergies;
    }

    public Allergen[] List()
    {
        List<Allergen> all = new List<Allergen>();
        foreach (Allergen a in Enum.GetValues<Allergen>())
        {
            if (IsAllergicTo(a)) all.Add(a);
        }
        return all.ToArray();
    }
}