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
    public static LedgerEntry CreateEntry(string date, string desc, int chng) =>
        new LedgerEntry(DateTime.Parse(date, CultureInfo.InvariantCulture), desc, chng / 100.0m);
    

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
            // check incoming localeString and instantiate CultureInfo object
            Culture = localeString switch
            {
                "en-US" or "nl-NL" => new CultureInfo(localeString),
                 _ => throw new ArgumentException("Invalid locale")
            };
            // check incoming currencyString and set CurrencySymbol on object
            Culture.NumberFormat.CurrencySymbol = currencyString switch
            {
                "USD" => "$",
                "EUR" => "€",
                _ => throw new ArgumentException("Invalid currency")
            };
            // LocType can be found by a ternary, will be used to set further formatting variables
            LocType = (localeString == "en-US" ? LocaleType.US : LocaleType.NL);
            // compiler warnings suppressed for these switch statements
            Culture.NumberFormat.CurrencyNegativePattern = LocType switch
            {
                LocaleType.US => 0,
                LocaleType.NL => 12
            };
            Culture.DateTimeFormat.ShortDatePattern = LocType switch
            {
                LocaleType.US => "MM/dd/yyyy",
                LocaleType.NL => "dd/MM/yyyy"
            };
        }
        public LocaleType LocType { get; }
        public CultureInfo Culture { get; }
    }
    
    // locale-dependent header
    private static string Header(LocaleType loc) => loc switch // compiler warning suppressed
    {
        LocaleType.US => "Date       | Description               | Change       ",
        LocaleType.NL => "Datum      | Omschrijving              | Verandering  "
    };

    // formatting constants and functions almost unchanged
    private const string Separator = " | ";
    private static string FormatDate(IFormatProvider Culture, DateTime date) => date.ToString("d", Culture);
    private static string FormatDescription(string desc) => (desc.Length > 25) ? desc.Substring(0, 22) + "..." : desc.PadRight(25);
    private static string FormatChange(IFormatProvider Culture, decimal cgh) =>
        (cgh < 0.0m ? cgh.ToString("C", Culture) : cgh.ToString("C", Culture) + " ").PadLeft(13);
    private static string FormatEntry(IFormatProvider Culture, LedgerEntry entry) =>
        $"{FormatDate(Culture, entry.Date)}{Separator}{FormatDescription(entry.Desc)}{Separator}{FormatChange(Culture, entry.Chg)}";
    
    // sorting function looks really better this way!
    private static IEnumerable<LedgerEntry> sort(LedgerEntry[] entries) =>
        entries.OrderBy(e => e.Date).ThenBy(e => e.Desc).ThenBy(e => e.Chg);

    // decisive public method with LinQ
    public static string Format(string currencyString, string localeString, LedgerEntry[] entries)
    {
        Locale locale = new Locale(localeString, currencyString);
        return string.Join("\n", 
            sort(entries)
            .Select(entry => FormatEntry(locale.Culture, entry))
            .Prepend(Header(locale.LocType))
        );
    }
}
