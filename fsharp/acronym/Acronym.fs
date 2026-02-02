module Acronym

open System

let abbreviate (phrase: string) = 
    phrase.Split([|' '; '-'; '_'|], StringSplitOptions.RemoveEmptyEntries)
    |> Seq.map (fun word -> word.[0] |> Char.ToUpper |> string)
    |> Seq.fold (fun acc c -> acc + c) ""