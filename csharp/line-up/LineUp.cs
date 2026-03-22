public static class LineUp
{
    public static string Format(string name, int number) =>
        $"{name}, you are the {number}{Suffix(number)} customer we serve today. Thank you!";
    
    private static string Suffix(int x) => x switch
    {
        int n when n % 10 == 1 && n % 100 != 11 => "st",
        int n when n % 10 == 2 && n % 100 != 12 => "nd",
        int n when n % 10 == 3 && n % 100 != 13 => "rd",
        _ => "th"
    };
}
