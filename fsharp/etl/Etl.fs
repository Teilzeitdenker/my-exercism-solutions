module Etl

open System

let toPoints = function
| 'A'| 'E'| 'I'| 'O'| 'U'| 'L'| 'N'| 'R'| 'S'| 'T' -> 1 
| 'D'| 'G'                                         -> 2
| 'B'| 'C'| 'M'| 'P'                               -> 3
| 'F'| 'H'| 'V'| 'W'| 'Y'                          -> 4
| 'K'                                              -> 5
| 'J'| 'X'                                         -> 8
| 'Q'| 'Z'                                         -> 10
| _   -> raise <| ArgumentException()

let transform (scoresWithLetters: Map<int, char list>): Map<char, int> = 
    scoresWithLetters 
    |> Map.toSeq 
    |> Seq.collect (fun (score, arr) -> arr |> Seq.map (fun (c: char) -> Char.ToLower(c), score )) 
    |> Map.ofSeq