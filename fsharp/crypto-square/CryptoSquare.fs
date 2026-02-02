module CryptoSquare

open System

let ciphertext input = 
    if input = "" then ""
    else 
        let normalized = 
            input
            |> Seq.filter Char.IsLetterOrDigit
            |> Seq.map Char.ToLowerInvariant
            |> Seq.toArray
            |> String
        let c = 
            normalized
            |> Seq.length
            |> float
            |> Math.Sqrt
            |> Math.Ceiling
            |> int 
        let chunks = 
            normalized
            |> Seq.chunkBySize c
            |> Seq.map String
            |> Seq.map (fun s -> s.PadRight c)
            |> Seq.toArray
        let transposed =
            [0..chunks.[0].Length-1] 
            |> Seq.map (fun n -> chunks |> Array.map (fun s -> s.[n]) |> String)
            |> Seq.toArray
        String.Join(" ", transposed)