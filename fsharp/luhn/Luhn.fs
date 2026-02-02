module Luhn

open System

let valid number = 
    let trimmed = number |> String.filter (Char.IsWhiteSpace >> not)
    if not (trimmed |> String.forall Char.IsDigit) || trimmed.Length < 2 then 
        false
    else 
        trimmed
        |> Seq.map (fun c -> Int32.Parse(c.ToString()))
        |> Seq.rev // reverse it
        |> Seq.mapi (fun i n -> 
            if i % 2 = 1 then  // do the "doubling" only for odd indices
                if n < 5 then 
                    2 * n 
                else 2 * n - 9 
            else n)
        |> Seq.sum 
        |> (%) <| 10  // use "<|" syntax to give 10 as the second argument 
        |> (=) 0