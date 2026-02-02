#pragma warning disable CS8524 // suppresses warnings because of non-exhaustive switch statements

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

// unchanged
public class LedgerEntry
{
   public LedgerEntry(DateTime date, string desc, decimal chg)
   {
       Date = date;
       Desc = desc;
       Chg = chg;
   }

   public DateTime Date { get; }
   public string Desc { get; }
   public decimal Chg { get; }
}


public static class Ledger
{
    // unchanged
    public static LedgerEntry CreateEntry(string date, string desc, int chng)
    {
        return new LedgerEntry(DateTime.Parse(date, CultureInfo.InvariantCulture), desc, chng / 100.0m);
    }

    // It's really a pity that C# enums won't exhaust
    // switch statements (as do the discriminated unions in F#),
    // since the underlying type here is an int and one could also define 
    // something like "LocaleType breakingThings = (LocaleType)5;"
    // I suppress the compiler warnings, since all this is private and 
    // the strings for locale and currency given from the outside are both checked!
    // Theres an interesting discussion going on around these issues, see
    // https://github.com/dotnet/csharplang/discussions/2671
    private enum LocaleType
    {
        US,
        NL
    }
    // saves a CultureInfo object that keeps all necessary information for formatting
    private class Locale
    {
        public Locale(string localeString, string currencyString)
        {
            // check incoming localeString and instantiate cultureInfo object
            culture = localeString switch
            {
                "en-US" or "nl-NL" => new CultureInfo(localeString),
                 _ => throw new ArgumentException("Invalid locale")
            };
            // check incoming currencyString and set CurrencySymbol on object
            culture.NumberFormat.CurrencySymbol = currencyString switch
            {
                "USD" => "$",
                "EUR" => "€",
                _ => throw new ArgumentException("Invalid currency")
            };
            // localeType can be found by a ternary, will be used to set further formatting variables
            localeType = (localeString == "en-US" ? LocaleType.US : LocaleType.NL);
            // compiler warnings suppressed for these switch statements
            culture.NumberFormat.CurrencyNegativePattern = localeType switch
            {
                LocaleType.US => 0,
                LocaleType.NL => 12
            };
            culture.DateTimeFormat.ShortDatePattern = localeType switch
            {
                LocaleType.US => "MM/dd/yyyy",
                LocaleType.NL => "dd/MM/yyyy"
            };
        }
        public LocaleType localeType { get; }
        public CultureInfo culture { get; set; }
    }
    
    // locale-dependent header
    private static string Header(LocaleType loc) => loc switch // compiler warning suppressed
    {
        LocaleType.US => "Date       | Description               | Change       ",
        LocaleType.NL => "Datum      | Omschrijving              | Verandering  "
    };

    // formatting constants and functions almost unchanged
    private const string Separator = " | ";
    private static string FormatDate(IFormatProvider culture, DateTime date) => date.ToString("d", culture);
    private static string FormatDescription(string desc) => (desc.Length > 25) ? desc.Substring(0, 22) + "..." : desc.PadRight(25);
    private static string FormatChange(IFormatProvider culture, decimal cgh) =>
        (cgh < 0.0m ? cgh.ToString("C", culture) : cgh.ToString("C", culture) + " ").PadLeft(13);
    private static string FormatEntry(IFormatProvider culture, LedgerEntry entry) =>
        FormatDate(culture, entry.Date) + Separator + FormatDescription(entry.Desc) + Separator + FormatChange(culture, entry.Chg);
    
    // sorting function looks really better this way!
    private static IEnumerable<LedgerEntry> sort(LedgerEntry[] entries) =>
        entries.OrderBy(e => e.Date).ThenBy(e => e.Desc).ThenBy(e => e.Chg);

    // decisive public method with LinQ
    public static string Format(string currencyString, string localeString, LedgerEntry[] entries)
    {
        Locale locale = new Locale(localeString, currencyString);
        return string.Join("\n", 
            sort(entries)
            .Select(entry => FormatEntry(locale.culture, entry))
            .Prepend(Header(locale.localeType))
        );
    }
}
