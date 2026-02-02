module LargestSeriesProduct

open System

let containsNonDigits = Seq.exists (Char.IsDigit >> not)

let largestProduct (input: string) (span: int) : int option = 
    let len = input |> String.length
    if span = 0 then Some 1
    elif span < 0 || len = 0 || span > len || containsNonDigits input then None
    else 
        let ints = input |> Seq.map (fun c -> int c - int '0')
        [0..(len-span)] 
        |> Seq.map (fun n -> ints |> Seq.skip n |> Seq.take span |> Seq.reduce (*))
        |> Seq.max
        |> Some
        