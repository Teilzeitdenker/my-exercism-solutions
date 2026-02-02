using System.Linq;
using System.Text.RegularExpressions;
public static class IsbnVerifier { public static bool IsValid(string number) => Regex.IsMatch(number, @"^(\d-?){9}(\d|X)$") && number.Where(char.IsLetterOrDigit).Select( (c, i) => (c == 'X' ? 10 : (int) char.GetNumericValue(c)) * (10 - i)).Sum()% 11 == 0; }