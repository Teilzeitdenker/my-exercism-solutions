using System;
using System.Diagnostics;

public static class RealNumberExtension
{
    public static double Expreal(this int realNumber, RationalNumber r)
    {
        return Math.Pow((double)realNumber, (double)r.num / (double)r.den);
    }
}

public struct RationalNumber
{
    public int num;
    public int den;
    public RationalNumber(int numerator, int denominator)
    {
        num = numerator;
        den = denominator;
    }

    public static RationalNumber operator +(RationalNumber r1, RationalNumber r2)
    {
        if (r1.den == r2.den) return new RationalNumber(r1.num + r2.num, r1.den).Reduce();
        return new RationalNumber(r1.num * r2.den + r2.num * r1.den, r1.den * r2.den).Reduce();
    }

    public static RationalNumber operator -(RationalNumber r1, RationalNumber r2)
    {
        if (r1.den == r2.den) return new RationalNumber(r1.num - r2.num, r1.den).Reduce();
        return new RationalNumber(r1.num * r2.den - r2.num * r1.den, r1.den * r2.den).Reduce();
    }

    public static RationalNumber operator *(RationalNumber r1, RationalNumber r2)
    {
        return new RationalNumber(r1.num * r2.num, r1.den * r2.den).Reduce();
    }

    public static RationalNumber operator /(RationalNumber r1, RationalNumber r2)
    {
        return (r1 * Inverse(r2)).Reduce();
    }

    public static RationalNumber Inverse(RationalNumber r)
    {
        return new RationalNumber(r.den, r.num);
    }

    public RationalNumber Abs()
    {
        return new RationalNumber(Math.Abs(num), Math.Abs(den));
    }

    public RationalNumber Reduce()
    {
        if (num == 0 || den == 1) return new RationalNumber(num, 1);
        int a = num;
        int b = den;
        while (a != 0)
        {
            int c = a;
            a = b % a;
            b = c;
        }
        if (b == 1) return this;
        if ( (b < 0 && den < 0) || (b > 0 && den > 0) ) return new RationalNumber(num / b, den / b);
        return new RationalNumber( - num / b, - den / b);
    }

    public RationalNumber Exprational(int power)
    {
        if (power == 0) return new RationalNumber(1, 1);
        if (power > 0) return new RationalNumber((int)Math.Pow((double)num, power), (int)Math.Pow((double)den, power)).Reduce();
        else return Inverse(Exprational(-power));
    }

    public double Expreal(int baseNumber)
    {
        return Math.Pow(baseNumber, ((double)num) / ((double)den));
    }
}