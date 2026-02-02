module SqueakyClean

open System

let transform c =
    match c with
    | ' '                         -> ""                    // no whitespace
    | c when Char.IsDigit(c)      -> ""                    // no digits
    | '-'                         -> "_"                   // hyphen -> underscore
    | c when Char.IsUpper(c)      -> $"-{Char.ToLower(c)}" // camelCase to kebab-case
    | c when c >= 'α' && c <= 'ω' -> "?"                   // lower greek letters to question marks
    | c                           -> c.ToString()          // all other chars unchanged (but as string)
    
let clean identifier =
    identifier |> Seq.fold (fun acc c -> acc + transform c) ""
