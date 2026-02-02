using System.Linq;
using System.Text.RegularExpressions;

public static class IsbnVerifier
{
    static Regex ValidIsbn = new Regex(@"^(\d-?){9}(\d|X)$");
    public static bool IsValid(string number)
    {
        if (!ValidIsbn.IsMatch(number)) return false;
        return number
                .Where(char.IsLetterOrDigit)
                .Select( (c, i) => (c == 'X' ? 10 : (int) char.GetNumericValue(c)) * (10 - i))
                .Sum() % 11 == 0;
    }
}