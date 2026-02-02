public struct CurrencyAmount
{
    private decimal amount;
    private string currency;

    public CurrencyAmount(decimal amount, string currency)
    {
        this.amount = amount;
        this.currency = currency;
    }

    public static bool operator ==(CurrencyAmount c1, CurrencyAmount c2)
    {
        if (c1.currency != c2.currency)
        {
            throw new System.ArgumentException();
        }
        else
        {
            return c1.amount == c2.amount;
        }        
    }

    public static bool operator !=(CurrencyAmount c1, CurrencyAmount c2)
    {
        return !(c1 == c2);
    }
    public static bool operator >=(CurrencyAmount c1, CurrencyAmount c2)
    {
        if (c1.currency != c2.currency)
        {
            throw new System.ArgumentException();
        }
        else
        {
            return c1.amount >= c2.amount;
        }
    }

    public static bool operator <(CurrencyAmount c1, CurrencyAmount c2)
    {
        return !(c1 >= c2);
    }

    public static bool operator <=(CurrencyAmount c1, CurrencyAmount c2)
    {
        if (c1.currency != c2.currency)
        {
            throw new System.ArgumentException();
        }
        else
        {
            return c1.amount <= c2.amount;
        }
    }

    public static bool operator >(CurrencyAmount c1, CurrencyAmount c2)
    {
        return !(c1 <= c2);
    }
    public static CurrencyAmount operator +(CurrencyAmount c1, CurrencyAmount c2)
    {
        if (c1.currency != c2.currency)
        {
            throw new System.ArgumentException();
        }
        else
        {
            return new CurrencyAmount(c1.amount + c2.amount, c1.currency);
        }
    }

    public static CurrencyAmount operator -(CurrencyAmount c1, CurrencyAmount c2)
    {
        if (c1.currency != c2.currency)
        {
            throw new System.ArgumentException();
        }
        else
        {
            return new CurrencyAmount(c1.amount - c2.amount, c1.currency);
        }
    }
    public static explicit operator double(CurrencyAmount c)
    {
        return (double)c.amount;
    }
    public static implicit operator decimal(CurrencyAmount c)
    {
        return c.amount;
    }
}
