module SqueakyClean

open System

let transform c =
    match c with
    | ' '                                 -> ""                                      // no whitespace
    | c when System.Char.IsDigit(c)       -> ""                                      // no digits
    | '-'                                 -> "_"                                     // hyphen -> underscore
    | c when System.Char.IsUpper(c)       -> "-" + System.Char.ToLower(c).ToString() // camelCase to kebab-case
    | c when int c >= 945 && int c <= 969 -> "?"                                     // lower greek letters to question marks
    | c                                   -> c.ToString()                            // all other cases unchanged (but as a string)
    
let clean identifier =
    identifier |> Seq.fold (fun acc c -> acc + transform c) ""
