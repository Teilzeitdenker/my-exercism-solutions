module Matrix

open System

let getMatrix (matrix: string) = 
    matrix.Split([|'\n'|]) |> Array.map (fun row -> row.Split([|' '|]) |> Array.map (Int32.Parse))

let row index matrix = 
    (getMatrix matrix).[index - 1] |> Array.toList

let column index matrix = 
    (getMatrix matrix) |> Array.map (fun row -> row.[index - 1]) |> Array.toList
