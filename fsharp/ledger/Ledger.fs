module Ledger

open System
open System.Globalization

// just left the Entry type and its 'constructor' as is, since test cases depend on them
type public Entry = { dat: DateTime; des: string; chg: int } // agnostic to locale and currency 

let public mkEntry (date: string) description change = 
    { dat = DateTime.Parse(date, CultureInfo.InvariantCulture); des = description; chg = change }

// add some private types to get complete pattern matches 
type private LocaleType   = US     | NL
type private CurrencyType = Dollar | Euro 

// use safe parsing in create function, also save some info for formatting benefits
type private Locale =
    { Type: LocaleType; Name: string; Format: string }
    static member create name =
        match name with
        | "en-US" -> { Type = US; Name = name; Format = "MM\/dd\/yyyy"}
        | "nl-NL" -> { Type = NL; Name = name; Format = "dd-MM-yyyy"}
        | _ -> failwith "Unexpected locale name"

type private Currency =
    { Type: CurrencyType; Name: string; Sign: string }
    static member create name =
        match name with
        | "USD" -> { Type = Dollar; Name = name; Sign = "$" }
        | "EUR" -> { Type = Euro  ; Name = name; Sign = "€" }
        | _ -> failwith "Unexpected currency name"


// formatting constants, one may also introduce a type Format with these fields inside which can be created 
// inside the formatLedges function and if not given, just uses these values as a default
let private dateWidth = 10
let private descrWidth = 25
let private changeWidth = 13
let private separator = " | "

// formatting functions, these depend heavily on the locale and currency objects given, but now there are (almost) no hardcoded values inside
let private header (locale: Locale) = 
    match locale.Type with
    | US -> "Date".PadRight(dateWidth)  + separator + "Description".PadRight(descrWidth)  + separator + "Change".PadRight(changeWidth)
    | NL -> "Datum".PadRight(dateWidth) + separator + "Omschrijving".PadRight(descrWidth) + separator + "Verandering".PadRight(changeWidth)

let private fmtDate (dat: DateTime) (locale: Locale) = (dat.ToString(locale.Format)).PadRight(dateWidth)

let private fmtDescr (des: string) = 
    match des with
    | _ when des.Length <= descrWidth -> des.PadRight(descrWidth)
    | _                               -> des.Substring(0, descrWidth - 3) + "..."

let private fmtAmount (change: int) (locale: Locale) (currency: Currency) =
    // collect some variables
    let cultInfo = new CultureInfo(locale.Name)
    let amt = (float change) / 100.00
    let isNegative = (amt < 0)
    // get different parts of the string
    let front =  // negative amounts are wrapped in braces in US locale, NL locale likes a space
        match (isNegative, locale.Type) with 
        | (false, US) ->       currency.Sign
        | (true,  US) -> "(" + currency.Sign
        | (_,     NL) ->       currency.Sign + " "
    let formattedDigits = // get rid of negative sign in US locale !
        match isNegative with 
        | true when locale.Type = US -> amt.ToString("#,#0.00", cultInfo).Substring(1)
        | _                          -> amt.ToString("#,#0.00", cultInfo)
    let back = // there is no real logic behind the first case, I think ...
        match (isNegative, locale.Type) with 
        | (false, _ ) -> " "
        | (true,  NL) -> ""
        | (true,  US) -> ")"
    // concat and apply padding
    (front + formattedDigits + back).PadLeft(changeWidth)

// use all this to format a complete entry of the ledger
let private fmtEntry (entry: Entry) (locale: Locale) (currency: Currency) =
    fmtDate entry.dat locale + separator +
    fmtDescr entry.des              + separator +
    fmtAmount entry.chg locale currency

// final public method
let public formatLedger currencyString localeString entries =
    // find the Locale and Currency types from the strings
    let locale   = Locale.create localeString
    let currency = Currency.create currencyString
    // get a list of strings for the sorted entries
    let lineList = 
        entries 
        |> List.sortBy (fun x -> x.dat, x.des, x.chg)
        |> List.map (fun entry -> fmtEntry entry locale currency) 
    // prepend the header and concat with newlines
    (header locale) :: lineList |> String.concat System.Environment.NewLine