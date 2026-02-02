module Pangram

open System

let isPangram (input: string): bool = 
    input |> Seq.filter Char.IsLetter |> Seq.map Char.ToLower |> Seq.distinct |> Seq.length = 26