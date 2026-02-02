module Transpose

open System

let rec transposeChars = function
    | (_::_)::_ as M -> List.map List.head M :: transposeChars (List.map List.tail M)
    | _ -> []

let transpose (input: string list) = 
    if List.length input = 0 then []
    else
        let maxlen = input |> Seq.map (fun word -> word.Length) |> Seq.max
        let charMatrix = input |> List.map (fun word ->  word.PadRight(maxlen, '*')) |> List.map (fun word -> word.ToCharArray() |> Array.toList)
        transposeChars charMatrix 
        |> Seq.map (fun charList -> charList |> Array.ofList |> String) 
        |> Seq.map (fun word -> word.TrimEnd('*').Replace('*', ' '))
        |> Seq.toList

    