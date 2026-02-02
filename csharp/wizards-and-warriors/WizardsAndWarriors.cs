using System;

abstract class Character
{
    protected string characterType = default;
    public bool vulnerable = default;
    protected Character(string characterType)
    {
        this.characterType = characterType;
    }

    public abstract int DamagePoints(Character target);

    public virtual bool Vulnerable()
    {
        return false;
    }

    public override string ToString()
    {
        return $"Character is a {this.characterType}";
    }
}

class Warrior : Character
{
    public Warrior() : base("Warrior")
    {
        // Warriors are never vulnerable
        this.vulnerable = false;
    }

    public override int DamagePoints(Character target)
    {
        if (target.vulnerable) return 10;
        else return 6;
    }
}

class Wizard : Character
{
    public Wizard() : base("Wizard")
    {
        // Wizards start vulnerable
        this.vulnerable = true;
    }

    public override bool Vulnerable()
    {
        if (this.vulnerable) return true;
        else return false;
    }

    public override int DamagePoints(Character target)
    {
        if (this.vulnerable) return 3;
        else return 12;
    }

    public void PrepareSpell()
    {
        // By preparing a spell, the wizard isn't vulnerable any more
        this.vulnerable = false;
    }
}
