using System.Collections.Generic;
using System.Linq;

public static class BottleSong
{
    public static IEnumerable<string> Recite(int startBottles, int takeDown)
    {
        var result = new List<string>();
        
        for (int i = 0; i < takeDown; i++)
        {
            int currentBottles = startBottles - i;
            int remainingBottles = currentBottles - 1;
            
            if (i > 0)
            {
                result.Add(""); // Empty line between verses
            }
            
            result.AddRange(GenerateVerse(currentBottles, remainingBottles));
        }
        
        return result;
    }
    
    private static IEnumerable<string> GenerateVerse(int currentBottles, int remainingBottles)
    {
        string currentBottleText = GetBottleText(currentBottles, true); // Capitalized for sentence start
        string remainingBottleText = GetBottleText(remainingBottles, false); // Lowercase for mid-sentence
        
        yield return $"{currentBottleText} hanging on the wall,";
        yield return $"{currentBottleText} hanging on the wall,";
        yield return "And if one green bottle should accidentally fall,";
        yield return $"There'll be {remainingBottleText} hanging on the wall.";
    }
    
    private static string GetBottleText(int count, bool capitalize)
    {
        return count switch
        {
            0 => "no green bottles",
            1 => capitalize ? "One green bottle" : "one green bottle",
            _ => $"{NumberToWord(count, capitalize)} green bottles"
        };
    }
    
    private static string NumberToWord(int number, bool capitalize)
    {
        string word = number switch
        {
            1 => "one",
            2 => "two", 
            3 => "three",
            4 => "four",
            5 => "five",
            6 => "six",
            7 => "seven",
            8 => "eight",
            9 => "nine",
            10 => "ten",
            _ => number.ToString()
        };
        
        return capitalize ? char.ToUpper(word[0]) + word[1..] : word;
    }
}
