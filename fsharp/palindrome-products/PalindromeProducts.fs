module PalindromeProducts

open System

let getSortedList minFactor maxFactor =
    [minFactor..maxFactor] 
    |> List.collect (fun i -> 
        [i..maxFactor] 
        |> List.filter (fun j -> 
            (string (i*j)) = (Seq.rev (string (i*j)) |> Seq.toArray |> String)) 
        |> List.map (fun j -> (i, j))) 
    |> List.groupBy (fun (i,j) -> i*j) 

let largest minFactor maxFactor = 
    if minFactor > maxFactor then
        raise <| ArgumentException() 
    else try 
            let (i, arr) = (getSortedList minFactor maxFactor) |> List.maxBy fst 
            (Some i, arr)
         with 
         | :? ArgumentException -> (None, [])

let smallest minFactor maxFactor = 
    if minFactor > maxFactor then
        raise <| ArgumentException()
    else try 
            let (i, arr) = (getSortedList minFactor maxFactor) |> List.minBy fst 
            (Some i, arr)
         with 
         | :? ArgumentException -> (None, [])