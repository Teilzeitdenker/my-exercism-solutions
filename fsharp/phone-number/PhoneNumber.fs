module PhoneNumber

open System

// Railway oriented programming
let (>>=) twoTrackInput switchFunction = 
    match twoTrackInput with
    | Ok s -> switchFunction s
    | Error e -> Error e

// Designing for errors using an ErrorMessage type as payload for the Error case
type ErrorMessage = 
    | InvalidInput of string
    | InvalidLength of string 
    | BadCountryCode
    | BadAreaCode of string
    | BadExchangeCode of string
    | ParsingError

let validateInput (input: string) = 
    if input |> Seq.exists (Char.IsLetter) then
        Error (InvalidInput "letters")
    elif input |> Seq.exists (fun c -> Char.IsPunctuation(c) && c <> '.' && c <> '-' && c <> '(' && c <> ')') then
        Error (InvalidInput "punctuations")
    else
        Ok input

let getCleanedList (input: string) =
    input |> Seq.filter (Char.IsDigit) |> Seq.toList |> Ok

let checkLength (input: char list) = 
    if Seq.length input > 11 then
        Error (InvalidLength "more than 11 digits")
    elif Seq.length input < 10 then
        Error (InvalidLength "incorrect number of digits") // better error message would be "less than 10 digits"
    else 
        Ok input

let checkCountryCode (input: char list) = 
    if (Seq.length input = 11) && (input.[0] <> '1') then
        Error BadCountryCode
    elif Seq.length input = 11 then
        Ok (input |> List.tail)  // reduce to the 10-digit case
    else 
        Ok input

let checkAreaCode (input: char list) =
    if input.[0] = '0' then 
        Error (BadAreaCode "zero")
    elif input.[0] = '1' then 
        Error (BadAreaCode "one")
    else 
        Ok input

let checkExchangeCode (input: char list) =
    if input.[3] = '0' then 
        Error (BadExchangeCode "zero")
    elif input.[3] = '1' then 
        Error (BadExchangeCode "one")
    else 
        Ok input

let tryParse (input: char list) = 
    match System.UInt64.TryParse(input |> Seq.toArray |> System.String) with
    | true, n -> Ok n
    | false, _ -> Error ParsingError

// generate the error messages 
let returnMessage result =
    match result with 
    | Ok n -> Ok n
    | Error err -> 
        match err with
        | InvalidInput str -> Error (sprintf "%s not permitted" str)
        | InvalidLength str -> Error str
        | BadCountryCode -> Error "11 digits must start with 1"
        | BadAreaCode str -> Error (sprintf "area code cannot start with %s" str)
        | BadExchangeCode str -> Error (sprintf "exchange code cannot start with %s" str) 
        | ParsingError -> Error "problem when parsing"

let clean input =
    input 
    |> validateInput
    >>= getCleanedList
    >>= checkLength
    >>= checkCountryCode
    >>= checkAreaCode
    >>= checkExchangeCode
    >>= tryParse
    |> returnMessage

// Solution with if, elif, else...

//let hasLetters (s:string) =
//    s |> Seq.exists (Char.IsLetter)
//let hasPunctuations (s:string) = 
//    s |> Seq.exists (fun c -> Char.IsPunctuation(c) && c <> '.' && c <> '-' && c <> '(' && c <> ')')

//let rec clean (input: string) : Result<uint64, string> = 
//    let d : char list = input |> Seq.filter (Char.IsDigit) |> Seq.toList
//    if hasLetters input then
//        Error "letters not permitted"
//    elif hasPunctuations input then 
//        Error "punctuations not permitted"
//    elif Seq.length d > 11 then
//        Error "more than 11 digits"
//    elif Seq.length d < 10 then
//        Error "incorrect number of digits"
//    elif (Seq.length d = 11) && (d.[0] <> '1') then
//        Error "11 digits must start with 1"
//    elif Seq.length d = 11 then
//        clean (d |> Seq.tail |> Seq.toArray |> System.String)
//    else
//        if d.[0] = '0' then 
//            Error "area code cannot start with zero"
//        elif d.[0] = '1' then 
//            Error "area code cannot start with one"
//        elif d.[3] = '0' then 
//            Error "exchange code cannot start with zero"
//        elif d.[3] = '1' then 
//            Error "exchange code cannot start with one"
//        else 
//            match System.UInt64.TryParse(d |> Seq.toArray |> System.String) with
//            | true, n -> Ok n
//            | false, _ -> Error "problem when parsing"
