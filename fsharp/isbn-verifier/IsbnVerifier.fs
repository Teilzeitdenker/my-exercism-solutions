module IsbnVerifier

open System
open System.Text.RegularExpressions

let isValid (isbn: string) : bool = 
    Regex.IsMatch(isbn, @"^(\d-?){9}(\d|X)$") &&
    (isbn 
        |> Seq.filter(Char.IsLetterOrDigit) 
        |> Seq.mapi(fun i c -> ( if c = 'X' then 10 else (int) (Char.GetNumericValue(c)) ) * (10 - i) )
        |> Seq.sum
    ) % 11 = 0