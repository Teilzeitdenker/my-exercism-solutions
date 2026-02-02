using System;

public static class Hamming
{
    public static int Distance(string firstStrand, string secondStrand)
    {
        int length = firstStrand.Length;
        if (secondStrand.Length != length) throw new ArgumentException();
        int count = 0;
        for (int i = 0; i < length; i++)
        {
            if (firstStrand[i] != secondStrand[i]) count++;
        }
        return count;
    }
}