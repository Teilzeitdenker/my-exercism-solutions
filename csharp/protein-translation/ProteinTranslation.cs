using System;
using System.Collections.Generic;

public static class ProteinTranslation
{
    public static string[] Proteins(string strand)
    {
        List<string> result = new List<string>();
        List<string> stopCodons = new List<string> { "UAA", "UAG", "UGA" };
        
        int numCodons = strand.Length / 3;
        Range r;
        
        for (int i = 0; i < numCodons; i++)
        {
            r = (3 * i)..(3 * i + 3);
            string actualCodon = strand[r];
            
            if (stopCodons.Contains(actualCodon))
                return result.ToArray();
            
            result.Add(actualCodon switch
            {
                "AUG" => "Methionine",
                "UUU" or "UUC" => "Phenylalanine",
                "UUA" or "UUG" => "Leucine",
                "UCU" or "UCC" or "UCA" or "UCG" => "Serine",
                "UAU" or "UAC" => "Tyrosine",
                "UGU" or "UGC" => "Cysteine",
                "UGG" => "Tryptophan",
                _ => throw new ArgumentOutOfRangeException()
            });
        }
        
        return result.ToArray();
    }
}