module ParallelLetterFrequency

open System

let frequency (texts: string list) = 
    texts 
    |> List.toArray
    |> Array.Parallel.collect (fun text -> 
        text.ToLower().ToCharArray()
        |> Array.filter (Char.IsLetter))
    |> Array.groupBy id
    |> Array.Parallel.map (fun (key, arr) -> (key, arr |> Array.length) )
    |> Map.ofArray 
        