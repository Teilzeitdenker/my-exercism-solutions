module BottleSong
open System

let private capitalize (str : string) =
    if String.length str = 0 then str else (Char.ToUpper str.[0] |> string) + str.[1..]

let private numbers = ["no"; "one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine"; "ten"]

let recite (startBottles : int) (takeDown : int) : string list =
    let bottleText (n : int) (cap : bool) =
        let nums = if not cap then numbers else numbers |> List.map capitalize
        $"{nums.[n]} green bottle" + if n <> 1 then "s" else ""
    let verse (n : int) =
        [ $"{bottleText n true} hanging on the wall,";
          $"{bottleText n true} hanging on the wall,";
          "And if one green bottle should accidentally fall,";
          $"There'll be {bottleText (n - 1) false} hanging on the wall." ]
    [startBottles .. -1 .. (startBottles - takeDown + 1)]
    |> List.map verse 
    |> List.reduce (fun acc v -> acc @ [""] @ v)
