module SqueakyClean

open System

let transform c =
    match c with
    | ' '                         -> ""                    // no whitespace
    | d when Char.IsDigit(d)      -> ""                    // no digits
    | '-'                         -> "_"                   // hyphen -> underscore
    | B when Char.IsUpper(B)      -> $"-{Char.ToLower(B)}" // camelCase to kebab-case
    | g when g >= 'α' && g <= 'ω' -> "?"                   // lower greek letters to question marks
    | _                           -> c.ToString()          // all other chars unchanged (but as string)
    
let clean identifier =
    identifier |> Seq.fold (fun acc c -> acc + transform c) ""
