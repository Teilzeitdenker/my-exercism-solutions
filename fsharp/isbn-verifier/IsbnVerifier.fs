module IsbnVerifier

open System.Text.RegularExpressions

let parseInt = function 
    | 'X' -> 10
    |  c  -> int c - int '0'

let isValid (isbn: string) : bool = 
    let numbers = isbn.Replace("-", "")
    Regex.IsMatch(numbers, @"^(\d){9}(\d|X)$") &&
    numbers 
        |> Seq.mapi(fun i c -> parseInt c * (10 - i) )
        |> Seq.sum 
        |> fun c -> c % 11 = 0