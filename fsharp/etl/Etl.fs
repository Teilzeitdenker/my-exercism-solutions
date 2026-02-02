module Etl

open System

let transform (scoresWithLetters: Map<int, char list>): Map<char, int> = 
    let scoreListPairToTuples ((score, ls): int * char list) =
        ls |> Seq.map (fun (c: char) -> Char.ToLower(c), score )
    scoresWithLetters |> Map.toSeq |> Seq.collect scoreListPairToTuples |> Map.ofSeq