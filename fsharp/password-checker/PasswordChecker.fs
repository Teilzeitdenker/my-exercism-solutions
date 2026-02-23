module PasswordChecker

open System

type PasswordError =
    | LessThan12Characters
    | MissingUppercaseLetter
    | MissingLowercaseLetter
    | MissingDigit
    | MissingSymbol

    
/// Validate the given password against the rules defined in the instructions. If it meets all
/// of the rules, return a result indicating success; otherwise return a result indicating
/// failure and an error indicating which rule was violated.
let checkPassword (password: string) : Result<string, PasswordError> =
    if password.Length < 12 then
        Error LessThan12Characters
    elif not (password |> Seq.exists Char.IsUpper) then
        Error MissingUppercaseLetter
    elif not (password |> Seq.exists Char.IsLower) then
        Error MissingLowercaseLetter
    elif not (password |> Seq.exists Char.IsDigit) then
        Error MissingDigit
    elif not (password |> Seq.exists (fun c -> "!@#$%^&*".Contains(c))) then
        Error MissingSymbol
    else
        Ok password

    
/// Return a human-readable message indicating the meaning of the given result value.
let getStatusMessage (result: Result<string, PasswordError>) : string =
    match result with
    | Ok _ -> "OK"
    | Error LessThan12Characters ->
        "Error: does not have at least 12 characters"
    | Error MissingUppercaseLetter ->
        "Error: does not have at least one uppercase letter"
    | Error MissingLowercaseLetter ->
        "Error: does not have at least one lowercase letter"
    | Error MissingDigit ->
        "Error: does not have at least one digit"
    | Error MissingSymbol ->
        "Error: does not have at least one symbol"
