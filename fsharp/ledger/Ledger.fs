module Ledger

open System
open System.Globalization

// just left the Entry type and its 'constructor' as is, since test cases depend on them
type public Entry = { dat: DateTime; des: string; chg: int } // agnostic to locale and currency 

let public mkEntry (date: string) description change = 
    { dat = DateTime.Parse(date, CultureInfo.InvariantCulture); des = description; chg = change }

// add types to get complete pattern matches 
type private Locale   = US     | NL
type private Currency = Dollar | Euro 

// formatting constants
let private dateWidth = 10
let private descrWidth = 25
let private changeWidth = 13
let private separator = " | "

// formatting functions
let private header = function
    | US -> "Date".PadRight(dateWidth)  + separator + "Description".PadRight(descrWidth)  + separator + "Change".PadRight(changeWidth)
    | NL -> "Datum".PadRight(dateWidth) + separator + "Omschrijving".PadRight(descrWidth) + separator + "Verandering".PadRight(changeWidth)

let private fmtDate (dat: DateTime) = function 
    | US -> (dat.ToString("MM\/dd\/yyyy")).PadRight(dateWidth)
    | NL -> (dat.ToString("dd-MM-yyyy")).PadRight(dateWidth)

let private fmtDescr (des: string) = 
    match des with
    | _ when des.Length <= descrWidth -> des.PadRight(descrWidth)
    | _                               -> des.Substring(0, descrWidth - 3) + "..."

let private fmtAmount (change: int) locale currency =
    // collect some variables
    let cultInfo = if locale = US then (new CultureInfo("en-US")) else (new CultureInfo("nl-NL"))
    let symb = 
        match currency with
        | Dollar -> "$"
        | Euro   -> "€"
    let amt = (float change) / 100.00
    let isNegative = (amt < 0)
    // get different parts of the string
    let front =  // negative amounts are wrapped in braces in US locale, NL locale likes a space
        match (isNegative, locale) with 
        | (false, US) ->       symb
        | (true,  US) -> "(" + symb
        | (_,     NL) ->       symb + " "
    let formattedDigits = 
        match isNegative with // get rid of negative sign in US locale
        | true when locale = US -> amt.ToString("#,#0.00", cultInfo).Substring(1)
        | _                     -> amt.ToString("#,#0.00", cultInfo)
    let back = // there is no real logic behind the first case, I think
        match (isNegative, locale) with 
        | (false, _ ) -> " "
        | (true,  NL) -> ""
        | (true,  US) -> ")"
    // concat and apply padding
    (front + formattedDigits + back).PadLeft(changeWidth)

// use all this to format a complete entry dependent on locale and currency
let private fmtEntry (entry: Entry) locale currency =
    fmtDate entry.dat locale + separator 
    + fmtDescr entry.des     + separator 
    + fmtAmount entry.chg locale currency

// final public method
let public formatLedger currencyString localeString entries =
    // find the Locale and Currency types from the strings
    let locale   = if localeString   = "en-US" then US     else NL 
    let currency = if currencyString = "USD"   then Dollar else Euro 
    // get a list of strings for the sorted entries
    let lineList = 
        entries 
        |> List.sortBy (fun x -> x.dat, x.des, x.chg)
        |> List.map (fun entry -> fmtEntry entry locale currency) 
    // prepend the header and concat with newlines
    (header locale) :: lineList |> String.concat System.Environment.NewLine